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
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET /posts
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _context.Posts.ToList();
        }

        // GET /posts/5
        [HttpGet("{id}")]
        public Post Get(int id)
        {
            Post post = _context.Posts.Find(id);
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
