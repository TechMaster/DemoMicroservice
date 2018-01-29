using System;

namespace blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public DateTime Published { get; set; }

        public bool IsActive { get; set; }
    }
}