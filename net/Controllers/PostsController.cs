using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers
{
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private List<Post> Posts = new List<Post> {
            new Post { Id = 1, Title = "Web API", Description = "Web API Description", Content = "Web API Content", Thumbnail = "/images/webapi.jpg", Published = new DateTime(2008,04,30), isActive = true},
            new Post { Id = 2, Title = "MVC", Description = "MVC Description", Content = "MVC Content", Thumbnail = "/images/mvc.jpg", Published = new DateTime(2000,07,20), isActive = false},
            new Post { Id = 3, Title = "Microservice", Description = "Microservice Description", Content = "Microservice Content", Thumbnail = "/images/microservice.jpg", Published = new DateTime(2001,11,08), isActive = true},
        };

        // GET /posts
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return Posts;
        }

        // GET /posts/5
        [HttpGet("{id}")]
        public Post Get(int id)
        {
            Post post = Posts.Where(p => p.Id == id).SingleOrDefault();
            return post;
        }

        // POST /posts
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT /posts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE /posts/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
