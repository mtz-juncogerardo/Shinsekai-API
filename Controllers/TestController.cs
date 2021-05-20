using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Test(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/")]
        public IActionResult Nothing()
        {
            return Ok(new OkResponse() 
            {
                Response = _configuration["Test"]
            });
        }

        [HttpGet("test")]
        public IActionResult GetItem()
        {
            return Ok(new OkResponse()
            {
                Response = "Test Works"
            });
        }
    }
}