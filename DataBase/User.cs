using System;

namespace DataBase
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Status { get; set; }
        public DateTime? LastSeen { get; set; }
        public bool? IsOnline { get; set; } = false;
        public bool? IsActive { get; set; } = true;

    }
}
