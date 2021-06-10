using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop.Infrastructure;
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
        private readonly IBlobService _blobService;

        public ImageController(ShinsekaiApiContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> SaveImages()
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var imageFile = Request.Form.Files[0];
            var imageItem = new ImageResponse
            {
                Id = Guid.NewGuid().ToString()
            };
            
            imageItem.Path = await _blobService.UploadContentBlobAsync(imageFile, imageItem.Id);

            if (imageItem.Path == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se pudo subir la imagen"
                });
            }

            return Ok(new OkResponse()
            {
                Response = imageItem
            });
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTempImages([FromQuery] string blob)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (blob == null)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "Define el nombre de la imagen a borrar"
                });
            }

            await _blobService.DeleteBlobAsync(blob);

            return Ok(new OkResponse()
            {
                Response = "DONE"
            });
        }
    }
}