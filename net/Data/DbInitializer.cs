using blog.Models;
using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace blog.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Posts.Any())
            {
                return;   // DB has been seeded
            }

            var posts = JsonConvert.DeserializeObject<List<Post>>(File.ReadAllText("Seed" + Path.DirectorySeparatorChar + "posts.json"));
            context.AddRange(posts);
            context.SaveChanges();
        }
    }
}