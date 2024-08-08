using DataBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clicker.Models
{
	public class LoginViewModel : DataViewModel
	{
		[Display(Name = "Логин")]
		[Required(ErrorMessage = "Неправильный логин")]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
		public override string Login { get; set; }

		[Display(Name = "Пароль")]
		[Required(ErrorMessage = "Неправильный пароль")]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "Логин не должен быть меньше 5 или больше 20 символов!")]
		public override string Password { get; set; }

	}
}
