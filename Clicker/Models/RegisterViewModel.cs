using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Clicker.Attributies;
using DataBase;

namespace Clicker.Models
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Придумайте логин")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Придумайте пароль")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
        public string Password { get; set; }

        public List<User> Users { get; set; }
    }
}




//     [CheatAttribute("Cheat", ErrorMessage = "Вы пока не активировали ключ")]  :  Свой лично созданный атрибут : @"Attributies/CommentAttribute"