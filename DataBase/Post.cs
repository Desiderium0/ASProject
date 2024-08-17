using System;

namespace DataBase
{
    public class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
