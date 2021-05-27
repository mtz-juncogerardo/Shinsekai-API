using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;
using Shinsekai_API.Services;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        private readonly IConfiguration _configuration;

        public ImageController(ShinsekaiApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveImage([FromForm] IFormFile myFile)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var blobService = new BlobStorageService("shinsekai", _configuration);
            var id = Guid.NewGuid().ToString();
            string path;
            try
            {
                 path = await blobService.UploadFileToStorage(myFile, id);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = e
                });
            }

            return Ok(new OkResponse()
            {
                Response = $"La imagen se guardo en el path: {path}"
            });
        }

    }
}