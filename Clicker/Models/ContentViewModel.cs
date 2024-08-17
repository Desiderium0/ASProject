using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clicker.Models
{
    public class ContentViewModel
    {
        [Display(Name = "Название темы")]
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Title { get; set; }

        [Display(Name = "Тема обсуждения")]
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [MinLength(10, ErrorMessage = "Тема должна быть не меньше 10 символов")]
        [MaxLength(500, ErrorMessage = "Тема должна быть не больше 500 символов")]
        public string Content { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        public DateTime DateTime { get; set; }

        public List<Post> Posts { get; set; }
    }
}
