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
    [Route("api/tags")]
    public class TagController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public TagController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetTags([FromQuery] string search, bool byAnime, bool byMaterial, bool byLine, bool byBrand)
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
            
            if (byBrand)
            {
                var dbBrands = _context.Brands.Where(b => b.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .ToList();
                return Ok(new OkResponse()
                {
                    Response = dbBrands
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
        [HttpPost("anime/create")]
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
            _context.Animes.Add(new AnimeItem() {Id = anime.Id, Name = anime.Name});
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Anime Tag has been saved"
            });
        }
        
        [Authorize]
        [HttpPost("brand/create")]
        public IActionResult SaveBrandTag([FromBody] BrandItem brand)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }
            
            brand.Id = Guid.NewGuid().ToString();
            _context.Brands.Add(new BrandItem() {Id = brand.Id, Name = brand.Name});
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Brand Tag has been saved"
            });
        }
        
        [Authorize]
        [HttpPost("line/create")]
        public IActionResult SaveLineTag([FromBody] LineItem line)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            line.Id = Guid.NewGuid().ToString();
            _context.Lines.Add(line);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Line Tag has been saved"
            });
        }
        
        [Authorize]
        [HttpPost("material/create")]
        public IActionResult SaveMaterialTag([FromBody] MaterialItem material)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            material.Id = Guid.NewGuid().ToString();
            _context.Materials.Add(material);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Material Tag has been saved"
            });
        }
        
        [Authorize]
        [HttpDelete("anime/delete")]
        public IActionResult DeleteAnimeTag([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una anime para eliminar"
                });
            }

            var dbAnime = _context.Animes.FirstOrDefault(q => q.Id == id);

            if (dbAnime == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbAnime);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La anime se elimino con exito"
            });
        }
        
        [Authorize]
        [HttpDelete("line/delete")]
        public IActionResult DeleteLineTag([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una line para eliminar"
                });
            }

            var dbLine = _context.Lines.FirstOrDefault(q => q.Id == id);

            if (dbLine == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbLine);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La line se elimino con exito"
            });
        }

        [Authorize]
        [HttpDelete("material/delete")]
        public IActionResult DeleteMaterialTag([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una material para eliminar"
                });
            }

            var dbMaterial = _context.Materials.FirstOrDefault(q => q.Id == id);

            if (dbMaterial == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbMaterial);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La material se elimino con exito"
            });
        }
        
        [Authorize]
        [HttpDelete("brand/delete")]
        public IActionResult DeleteBrandTag([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una brand para eliminar"
                });
            }

            var dbBrand = _context.Brands.FirstOrDefault(q => q.Id == id);

            if (dbBrand == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Brand que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbBrand);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La brand se elimino con exito"
            });
        }
    }
}