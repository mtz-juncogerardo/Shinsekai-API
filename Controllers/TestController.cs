using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    public class Test : ControllerBase
    {

        [HttpGet]
        public IActionResult Nothing()
        {
            return Ok(new OkResponse() 
            {
                Response = "Ok"
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