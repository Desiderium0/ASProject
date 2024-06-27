using Clicker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataBase;

namespace Clicker.Controllers
{
    public class HomePageController : Controller
    {
        private ApplicationContext _dataBase = null!;

        public IActionResult Index(RegisterViewModel registerModel)
        {
            return View(registerModel);
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
                    registerModel.Users = _dataBase.Users.ToList();
                    return View("Index", registerModel);
                }
                registerModel.Users = _dataBase.Users.ToList();
                return View("Index", registerModel);
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
            return View("Index", registerModel);
        }

        public IActionResult Shop()
        {
            return View();
        }
    }
}