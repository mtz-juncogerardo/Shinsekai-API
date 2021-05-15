using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("tag")]
    public class TagController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public TagController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetTags([FromQuery] string search, bool byAnime, bool byMaterial, bool byLine)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (byAnime)
            {
                var dbAnimes = _context.Animes.Where(a => a.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .ToList();
                return Ok(new OkResponse()
                {
                    Response = dbAnimes
                });
            }

            if (byMaterial)
            {
                var dbMaterials = _context.Materials.Where(m => m.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .ToList();
                return Ok(new OkResponse()
                {
                    Response = dbMaterials
                });
            }

            if (byLine)
            {
                var dbLines = _context.Lines.Where(l => l.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .ToList();
                return Ok(new OkResponse()
                {
                    Response = dbLines
                });
            }

            return BadRequest( new ErrorResponse()
            {
                Error = "No Category specified"
            });
        }

        [Authorize]
        [HttpPost("anime")]
        public IActionResult SaveAnimeTag([FromBody] AnimeItem anime)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            anime.Id = Guid.NewGuid().ToString();
            _context.Animes.Add(anime);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Anime Tag has been saved"
            });
        }
    }
}