using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public AdminController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet("questions/read")]
        public IActionResult GetQuestions([FromQuery] string id)
        {
            var dbQuestion = _context.Questions.ToList();
            if (id == null)
            {
                return Ok(new OkResponse()
                {
                    Response = dbQuestion,
                    Count = dbQuestion.Count,
                    Page = 1,
                    MaxPage = 1
                });
            }

            var dbResponse = dbQuestion.Where(r => r.Id == id);
            return Ok(new OkResponse()
            {
                Response = dbResponse
            });
        }

        [Authorize]
        [HttpPost("questions/create")]
        public IActionResult SaveQuestion([FromBody] QuestionItem question)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            question.Id = Guid.NewGuid().ToString();
            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = question
            });
        }

        [Authorize]
        [HttpPut("questions/update")]
        public IActionResult UpdateQuestion([FromBody] QuestionItem question)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (question.Id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Id not specified"
                });
            }

            var entityExistsFlag = _context.Questions.Any(q => q.Id == question.Id);

            if (!entityExistsFlag)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Question Invalid Id"
                });
            }

            _context.Update(question);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = question
            });
        }

        [Authorize]
        [HttpDelete("questions/delete")]
        public IActionResult DeleteQuestion([FromQuery] string id)
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
                    Error = "No se especifico una pregunta para eliminar"
                });
            }

            var dbQuestion = _context.Questions.FirstOrDefault(q => q.Id == id);

            if (dbQuestion == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbQuestion);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La pregunta se elimino con exito"
            });
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var id = AuthService.IdentifyUser(User.Identity);

            var dbUsers = _context.Users.ToList();

            return Ok(new OkResponse()
            {
                Response = dbUsers
            });
        }

        [Authorize]
        [HttpPut("users")]
        public IActionResult UpdateAdminUser(UserItem user)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (dbUser == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El usuario ya no existe"
                });
            }

            if (user.Admin == dbUser.Admin)
            {
                return Ok();
            }

            dbUser.Admin = user.Admin;

            _context.Update(dbUser);
            _context.SaveChanges();

            var query = dbUser.Admin ? $"Se le han concedido permisos de administrador al usuario: {dbUser.Name ?? dbUser.Email}" : $"Se le retiraron los permisos para administrar al usuario: {dbUser.Name ?? dbUser.Email}";


            return Ok(new OkResponse()
            {
                Response = query 
            });
        }
    }
}