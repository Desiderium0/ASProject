using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DataBase;
using System.ComponentModel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Clicker.Controllers
{
	public class HomePageController : Controller
	{
		private ApplicationContext _dataBase = null!;

		#region FormRendering 
		public IActionResult ContentManager()
		{
			return View();
		}

		public IActionResult RegisterForm()
		{
			return View();
		}

		public IActionResult LoginForm()
		{
			return View();
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
						byte[] storedHashBytes = Convert.FromBase64String(data.Password);
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
						Salt = Convert.ToBase64String(salt),
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
		#endregion
	}
}