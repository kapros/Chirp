using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chirp.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class SecretController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok("You should only see this if you have a special key");
        }
    }
}
