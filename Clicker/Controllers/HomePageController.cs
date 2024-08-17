using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using DataBase;
using System.ComponentModel;

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

		public IActionResult ContentManager(ContentViewModel contentModel)
		{
			using (_dataBase = new ApplicationContext())
			{
                contentModel.Posts = _dataBase.Posts.ToList();
				return View(contentModel);
			}
		}
		public IActionResult Home(LoginViewModel loginModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				loginModel.Users = _dataBase.Users.ToList();
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
					var list = _dataBase.Users
						.Select(x => new { x.Login, x.Password, x.Salt })
						.ToList();

					foreach (var data in list)
					{
						byte[] storedHashBytes = Convert.FromBase64String(data.Password); //вынести в отдельный метод
						byte[] saltBytes = Convert.FromBase64String(data.Salt);

						string password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: loginModel.Password!,
							salt: saltBytes,
							prf: KeyDerivationPrf.HMACSHA256,
							iterationCount: 100000,
							numBytesRequested: 256 / 8)
						);

						if (password == Convert.ToBase64String(storedHashBytes)
							&& data.Login == loginModel.Login)
						{ 
							return View("Home", UploadTable(loginModel));
						}
					}
				}
			}
			return View("LoginForm");
		}

		[HttpPost]
		public IActionResult CreateUser(RegisterViewModel registerModel)
		{
			byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
			using (_dataBase = new ApplicationContext())
			{
				_dataBase.Database.EnsureCreated();
				if (ModelState.IsValid)
				{	
					var user = new User
					{
						Salt = Convert.ToBase64String(salt),                //вынести в отдельный метод
						Login = registerModel.Login,
						Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: registerModel.Password!,
							salt: salt,
							prf: KeyDerivationPrf.HMACSHA256,
							iterationCount: 100000,
							numBytesRequested: 256 / 8))
					};
					_dataBase.Users.Add(user);
					_dataBase.SaveChanges();
					
					return View("Home", UploadTable(registerModel));
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
                }
                return View("ContentManager", UploadTable(contentModel));
            }   
		}

		#endregion

		#region Methods
		[Description("Метод для заполнения таблицы, обработкой моделью Login")]
		public LoginViewModel UploadTable(LoginViewModel loginModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				loginModel.Users = _dataBase.Users.ToList();
				return loginModel;
			}
		}

		[Description("Метод для заполнения таблицы, обработкой моделью Register")]
		public RegisterViewModel UploadTable(RegisterViewModel registerModel)
		{
			using (_dataBase = new ApplicationContext())
			{
				registerModel.Users = _dataBase.Users.ToList();
				return registerModel;
			}
		}

        public ContentViewModel UploadTable(ContentViewModel contentModel)
        {
            using (_dataBase = new ApplicationContext())
            {
                contentModel.Posts = _dataBase.Posts.ToList();
                return contentModel;
            }
        }
        #endregion
    }
}