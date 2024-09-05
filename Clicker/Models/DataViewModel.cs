using DataBase;
using System.Collections.Generic;

namespace Clicker.Models
{
    public abstract class DataViewModel
    {
        public int Id { get; }

        public abstract string Login { get; set; }

        public abstract string Password { get; set; }

        public List<User> Users { get; set; }

        public User CurrentUser { get; set; }
    }
}
