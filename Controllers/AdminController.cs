using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;

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


        [Authorize]
        [HttpPost("questions")]
        public IActionResult SaveQuestion([FromBody] QuestionItem question)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new
                {
                    Error = "You dont have the required role"
                });
            }

            question.Id = Guid.NewGuid().ToString();
            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok(new
            {
                Result = "New Question Published"
            });
        }

    }
}