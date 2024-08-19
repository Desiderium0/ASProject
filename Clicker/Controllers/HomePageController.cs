using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using DataBase;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		public IActionResult LoginUser(LoginViewModel loginModel) 
		{
			using (_dataBase = new ApplicationContext())
			{
				if (ModelState.IsValid)
				{
                    var user = _dataBase.Users.FirstOrDefault(x => x.Login == loginModel.Login);

                    if (user != null && VerifyPassword(loginModel.Password, user.PasswordHash, user.Salt))
                    {
                        return View("Home", UploadTable(loginModel));
                    }
                }
			}
			return View("LoginForm");
		}

		[HttpPost]
		public IActionResult CreateUser(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				_dataBase.Database.EnsureCreated();
				if (ModelState.IsValid)
				{
					var (salt, passwordHash) = GenerateSaltAndPasswordHash(registerModel.Password);
                    var user = new User
                    {
                        Salt = salt,
                        Login = registerModel.Login,
                        PasswordHash = passwordHash
                    };
                    _dataBase.Users.Add(user);
					_dataBase.SaveChanges();
					
					return RedirectToAction("Home"); 
				}
				return View("RegisterForm");
			}
		}

		[HttpPost]
		public IActionResult DeleteUsers(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				foreach (var item in _dataBase.Users)
				{ 
					_dataBase.Remove(item);
				}
				_dataBase.SaveChanges();
			}
			return View("Home", registerModel);
		}

        [HttpPost]
        public IActionResult CreatePost(ContentViewModel contentModel)
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
					_dataBase.Posts.Add(post);
					_dataBase.SaveChanges();

                    return RedirectToAction("ContentManager");
                }
				return View("ContentManager", UploadTable(contentModel));
			}   
		}

		#endregion

		#region Methods
		[Description("Заполняет таблицу данными выходя из формы авторизации")]
        private LoginViewModel UploadTable(LoginViewModel loginModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				loginModel.Users = _dataBase.Users.ToList();
				return loginModel;
			}
		}

		[Description("Заполняет таблицу данными выходя из формы регистрации")]
        private RegisterViewModel UploadTable(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				registerModel.Users = _dataBase.Users.ToList();
				return registerModel;
			}
		}

        [Description("Заполняет таблицу данными для постов")]
        private ContentViewModel UploadTable(ContentViewModel contentModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				contentModel.Posts = _dataBase.Posts.ToList();
				return contentModel;
			}
		}

        private bool VerifyPassword(string password, byte[] storedHashBytes, byte[] saltBytes)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return hashedPassword == Convert.ToBase64String(storedHashBytes);
        }

        private (byte[], byte[]) GenerateSaltAndPasswordHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

			byte[] bytesPasswordHash = Convert.FromBase64String(passwordHash);

            return (salt, bytesPasswordHash);
        }

        #endregion
    }
}