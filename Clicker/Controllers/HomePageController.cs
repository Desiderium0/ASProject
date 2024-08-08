using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DataBase;
using System.ComponentModel;

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
						.Select(x => new { x.Login, x.Password })
						.ToList();

					foreach (var data in list)
					{
						if (data.Password == loginModel.Password
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
			using (_dataBase = new ApplicationContext())
			{
				_dataBase.Database.EnsureCreated();
				if (ModelState.IsValid)
				{
					var user = new User
					{
						Login = registerModel.Login,
						Password = registerModel.Password
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