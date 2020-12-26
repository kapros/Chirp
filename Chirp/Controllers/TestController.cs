using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new { Message = "Pong" });
        }
    }
}
