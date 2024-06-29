using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Clicker.Attributies;
using DataBase;

namespace Clicker.Models
{
    public class RegisterViewModel : DataViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Придумайте логин")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
        public override string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Придумайте пароль")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
        public override string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}