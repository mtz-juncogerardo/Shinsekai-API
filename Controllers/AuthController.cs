using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Authentication;
using Shinsekai_API.MailSender;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ShinsekaiApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost ("authorize")]
        public IActionResult AuthorizeUser(UserItem user)
        {
            if (user.Address == null || user.City == null || user.Name == null || user.Phone == null)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "You must specify an Adress, a City, a Name and a Phone so we can authorize to buy"
                });
            }
            var jwt = new JsonWebTokenAuth(user.Id, user.Email, _configuration,true);

            return Ok(new OkResponse()
            {
                 Response = jwt.Token
            });
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] AuthParams authParams)
        {
            var passwordService = new PasswordService(authParams.Password);
            if (!passwordService.ValidatePassword())
            {
                return BadRequest(new
                {
                    Error = "Password does not met specified parameters"
                });
            }
            var dbUser = _context.Users.FirstOrDefault(r => r.Email == authParams.Email);
            if (dbUser == null)
            {
                return Unauthorized(new
                {
                    Error = "Account is not active or password has changed"
                });
            }
            dbUser.AuthParams = _context.AuthParams.FirstOrDefault(r => r.Id == dbUser.AuthParamsId);
            var auth = new AuthService(authParams.Email, authParams.Password);
            var token = auth.AuthByEmailAndPassword(dbUser, _configuration);
            if (token == null)
            {
                return Unauthorized(new
                {
                    Error = "Account is not active or password has changed"
                });
            }
            return Ok(new
            {
                Response = "Logged correctly",
                Token = token
            });
        }

        [HttpPost("recover")]
        public IActionResult RecoverPassword([FromBody] AuthParams authParams)
        {
            var dbUser = _context.Users.FirstOrDefault(r => r.Email == authParams.Email);
            if (dbUser == null)
            {
                return NotFound(new
                {
                    Error = "Email does not match any known records"
                });
            }
            var jwt = new JsonWebTokenAuth(dbUser.Id, dbUser.Email, _configuration, true);
            var recoverCredentials = new RecoverCredentialsMail(dbUser.Email, jwt.Token, _configuration);
            recoverCredentials.SendEmail();
            return Ok(new
            {
                Response = "An Email has been sent with futher instructions",
            });
        }

        [HttpPost("signup")]
        public IActionResult SignUpUser([FromBody] AuthParams authParams)
        {
            var passwordService = new PasswordService(authParams.Password);
            if (!passwordService.ValidatePassword())
            {
                return BadRequest(new
                {
                    Error = "Password does not met specified parameters"
                });
            }
            var dbUser = _context.Users.FirstOrDefault(r => r.Email == authParams.Email);
            if (dbUser != null)
            {
                return BadRequest(new
                {
                    Error = "Email already exists"
                });
            }
            dbUser = new UserItem()
            {
                Id = Guid.NewGuid().ToString(),
                Email = authParams.Email
            };
            var jwt = new JsonWebTokenAuth(dbUser.Id,
                                           dbUser.Email,
                                           passwordService.Salt, passwordService.HashPassword, _configuration);
            var link = $"http://localhost:4200/register?token={jwt.Token}";
            var emailValidation = new EmailValidationMail(dbUser.Email, link, _configuration);
            emailValidation.SendEmail();

            return Ok(new
            {
                Response = "We sent you an email, please confirm it before continuing",
            });
        }

        [Authorize]
        [HttpPost("register")]
        public IActionResult RegisterUser()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var userClaim = (ClaimsIdentity)User.Identity ?? new ClaimsIdentity();
            var email = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .First();
            var password = userClaim.Claims.Where(r => r.Type == ClaimTypes.Hash)
                .Select(c => c.Value)
                .First();
            var salt = userClaim.Claims.Where(r => r.Type == ClaimTypes.Authentication)
                .Select(c => c.Value)
                .First();
            var dbUser = _context.Users.FirstOrDefault(r => r.Email == email);
            if (dbUser != null)
            {
                return Ok(new
                {
                    Response = dbUser
                });
            }
            if (password == string.Empty || salt == string.Empty)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "Token doesnt include auth params"
                });
            }
            var authParams = new AuthParamItem()
            {
                Id = Guid.NewGuid().ToString(),
                Salt = salt,
                Password = password
            };
            dbUser = new UserItem()
            {
                Id = id,
                Email = email,
                AuthParams = authParams,
            };
            _context.Users.Add(dbUser);
            _context.SaveChanges();

            return Ok(new
            {
                Response = "User created correctly"
            });
        }

        [Authorize]
        [HttpPut("security")]
        public IActionResult UpdateUser([FromBody] AuthParams authParams)
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbUser = _context.Users.FirstOrDefault(r => r.Id == id);
            if (dbUser == null)
            {
                return Unauthorized(new
                {
                    Response = "Token is Corrupted or has expired"
                });
            }
            dbUser.AuthParams = _context.AuthParams.FirstOrDefault(r => r.Id == dbUser.AuthParamsId) ?? new AuthParamItem();
            if (authParams.Password != null)
            {
                var passwordService = new PasswordService(authParams.Password);
                if (!passwordService.ValidatePassword())
                {
                    return BadRequest(new ErrorResponse() 
                    {
                        Error = "Password doesnt met specified parameters"
                    });
                }
                dbUser.AuthParams.Password = passwordService.HashPassword;
                dbUser.AuthParams.Salt = passwordService.Salt;
            }
            if (authParams.Email != null)
            {
                dbUser.Email = authParams.Email;
            }
            _context.SaveChanges();
            return Ok(new
            {
                Response = "Your data has been updated"
            });
        }
    }
}