using DataBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clicker.Models
{
    public class LoginViewModel : DataViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Неправильный логин")]
        public override string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Неправильный пароль")]
        public override string Password { get; set; }

    }
}
