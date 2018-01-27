using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        // GET /posts
        [HttpGet]
        public string Get()
        {
            return "Blog APIs";
        }
    }
}
