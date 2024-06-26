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
        public IActionResult Index(RegisterViewModel registerModel)
        {
            return View(registerModel);
        }

        [HttpPost]
        public IActionResult CreateUser(RegisterViewModel registerModel)
        {
            using (ApplicationContext dataBase = new ApplicationContext())
            {
                dataBase.Database.EnsureCreated();
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Login = registerModel.Login,
                        Password = registerModel.Password
                    };
                    dataBase.Users.Add(user);
                    dataBase.SaveChanges();
                    registerModel.Users = dataBase.Users.ToList();
                    return View("Index", registerModel);
                }
                registerModel.Users = dataBase.Users.ToList();
                return View("Index", registerModel);
            }
        }

        [HttpPost]
        public IActionResult DeleteUsers(RegisterViewModel registerModel)
        {
            using (ApplicationContext dataBase = new ApplicationContext())
            {
                foreach (var item in dataBase.Users)
                {
                    dataBase.Remove(item);
                }
                dataBase.SaveChanges();
            }
            return View("Index", registerModel);
        }

        public IActionResult Shop()
        {
            return View();
        }
    }
}