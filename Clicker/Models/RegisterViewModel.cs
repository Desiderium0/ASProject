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
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Логин не должен быть меньше 8 символов!")]
        [RegularExpression(@"(?=.*[A-Za-z0-9]).*\S", ErrorMessage = "Логин не должен содержать спец. символов!")]
        public override string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Придумайте пароль")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[0-9])(?=.*[#$%&'*+:;<=>?@\\_\\s]).*", ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву, одну цифру и один из символов: !\\\"!#$%&'*+:;<=>?@\\_")]
        public override string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}