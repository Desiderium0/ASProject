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
            
            using (ApplicationContext db = new ApplicationContext())
            {
                registerModel.Users = db.Users.ToList();
                
            }

            return View(registerModel);
        }

        public IActionResult Shop()
        {
            return View();
        }

        public void CreateInstance(RegisterViewModel registerModel)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Database == null)
                {
                    db.Database.EnsureCreated();
                }
                
                // создаем два объекта User
                User user = new User
                {
                    Password = registerModel.Password,
                    Login = registerModel.Login
                };

                // добавляем их в бд
                db.Users.AddRange(user);
                db.SaveChanges();
            }
        }
    }
}