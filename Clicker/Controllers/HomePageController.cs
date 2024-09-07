using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using DataBase;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Clicker.Controllers
{
	public class HomePageController : Controller
	{
		private ApplicationContext _dataBase = null!;

		#region FormRendering 

		public IActionResult RegisterForm()
		{
			return View();
		}

		public IActionResult LoginForm()
		{
			return View();
		}

		public IActionResult ContentManager()
		{
			using (_dataBase = new ApplicationContext())
			{
				var posts = _dataBase.Posts.ToList();

				ContentViewModel contentModel = new ContentViewModel
				{
					Posts = posts
				};

                return View(contentModel);
			}
		}
		public IActionResult Home()
		{
			using (_dataBase = new ApplicationContext())
			{
				var users = _dataBase.Users.ToList();
			//	var currentUser = _dataBase.Users.LastOrDefault(x => x.Login == );

				LoginViewModel loginModel = new LoginViewModel
				{
					Users = users
				};
			
                return View(loginModel);
			}
		}
		#endregion

		#region Actions
		[HttpPost]
		public async Task<IActionResult> LoginUser(LoginViewModel loginModel) 
		{
			using (_dataBase = new ApplicationContext())
			{
				if (ModelState.IsValid)
				{
                    var user = await _dataBase.Users.FirstOrDefaultAsync(x => x.Login == loginModel.Login);
					bool isCorrect = await VerifyPasswordAsync(loginModel.Password, user.PasswordHash, user.Salt);
                    if (user != null && isCorrect)
                    {
                        return View("Home", UploadTableAsync(loginModel));
                    }
                }
			}
			return View("LoginForm");
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				await _dataBase.Database.EnsureCreatedAsync();

				var isDublicate = await _dataBase.Users.FirstOrDefaultAsync(x => x.Login == registerModel.Login);
				if (ModelState.IsValid && isDublicate == null)
				{
					var (salt, passwordHash) = await GenerateSaltAndPasswordHashAsync(registerModel.Password);
                    var user = new User
                    {
                        Salt = salt,
                        Login = registerModel.Login,
                        PasswordHash = passwordHash
                    };
                    
                    await _dataBase.Users.AddAsync(user);
                    await _dataBase.SaveChangesAsync();
					
					return RedirectToAction("Home"); 
				}
				else if(isDublicate != null)
				{
					ModelState.AddModelError("Login", "Такой пользователь существует");
					return View("RegisterForm");
                }
				else
				{
                    return View("RegisterForm");
                }
			}
		}

		[HttpPost]
		public IActionResult DeleteUsers(RegisterViewModel registerModel)              //ТЕСТОВЫЙ метод, через время удалю (АСИНХРОННОСТИ НЕ БУДЕТ)
		{
			using (_dataBase = new ApplicationContext())
			{
				foreach (var item in _dataBase.Users)
				{ 
					_dataBase.Remove(item);
				}
                _dataBase.UpdateRange();
                _dataBase.SaveChanges();
            }
			return View("Home", registerModel);
		}

        [HttpPost]
        public async Task<IActionResult> CreatePost(ContentViewModel contentModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				if (ModelState.IsValid)
				{
					var post = new Post
					{
						Name = contentModel.Title,
						Content = contentModel.Content,
						Author = contentModel.Author,
						TimeCreated = DateTime.Now
					};
					await _dataBase.Posts.AddAsync(post);
					await _dataBase.SaveChangesAsync();

                    return RedirectToAction("ContentManager");
                }
				return View("ContentManager", await UploadTableAsync(contentModel));
			}   
		}

		#endregion

		#region Methods
		[Description("Заполняет таблицу данными выходя из формы авторизации")]
        private async Task<LoginViewModel> UploadTableAsync(LoginViewModel loginModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				loginModel.Users = await _dataBase.Users.ToListAsync();
				return loginModel;
			}
		}

		[Description("Заполняет таблицу данными выходя из формы регистрации")]
        private async Task<RegisterViewModel> UploadTableAsync(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				registerModel.Users = await _dataBase.Users.ToListAsync();
				return registerModel;
			}
		}

        [Description("Заполняет таблицу данными для постов")]
        private async Task<ContentViewModel> UploadTableAsync(ContentViewModel contentModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				contentModel.Posts = await _dataBase.Posts.ToListAsync();
				return contentModel;
			}
		}

        private async Task<bool> VerifyPasswordAsync(string password, byte[] storedHashBytes, byte[] saltBytes)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return await Task.FromResult(hashedPassword == Convert.ToBase64String(storedHashBytes));
        }

        private async Task<(byte[], byte[])> GenerateSaltAndPasswordHashAsync(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

			byte[] bytesPasswordHash = Convert.FromBase64String(passwordHash);

            return await Task.FromResult((salt, bytesPasswordHash));
        }

        #endregion
    }
}