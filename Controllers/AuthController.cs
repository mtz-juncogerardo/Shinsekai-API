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
        public AuthController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("admin")]
        public IActionResult GetAuthorization()
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            return Ok(new OkResponse()
            {
                Response = "Authorized"
            });
        }

        [HttpPost("authorize")]
        public IActionResult AuthorizeUser(UserItem user)
        {
            if (user.Address == null || user.City == null || user.Name == null || user.Phone == null || user.PostalCode == null || user.Email == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "You must specify an Adress, a City, a Name, an email and a Phone so we can authorize to buy"
                });
            }

            user.Id = Guid.NewGuid().ToString();
            var jwt = new JsonWebTokenAuth(user, true);

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
                return Unauthorized(new ErrorResponse()
                {
                    Error = "La cuenta no esta activa o la contraseña ha cambiado"
                });
            }
            dbUser.AuthParams = _context.AuthParams.FirstOrDefault(r => r.Id == dbUser.AuthParamsId);
            var auth = new AuthService(authParams.Email, authParams.Password);
            var token = auth.AuthByEmailAndPassword(dbUser);
            if (token == null)
            {
                return Unauthorized(new
                {
                    Error = "Account is not active or password has changed"
                });
            }
            return Ok(new OkResponse()
            {
                Response = token
            });
        }

        [HttpPost("recover")]
        public IActionResult RecoverPassword([FromBody] AuthParams authParams)
        {
            var dbUser = _context.Users.FirstOrDefault(r => r.Email == authParams.Email);
            if (dbUser == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Error = "No hay ninguna cuenta asociada con el email proporcionado"
                });
            }
            var jwt = new JsonWebTokenAuth(dbUser.Id, dbUser.Email, true);
            var link = $"https://shinsekai.mx/recovery/{jwt.Token}";
            var recoverCredentials = new RecoverCredentialsMail(dbUser.Email, link);
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
            if (dbUser != null && dbUser.AuthParamsId != null)
            {
                return BadRequest(new
                {
                    Error = "El email ya se encuentra registrado, intenta iniciar sesión"
                });
            }
            dbUser = new UserItem()
            {
                Id = dbUser != null ? dbUser.Id : Guid.NewGuid().ToString(),
                Email = authParams.Email
            };
            var jwt = new JsonWebTokenAuth(dbUser.Id,
                dbUser.Email,
                passwordService.HashPassword, passwordService.Salt);
            var link = $"https://shinsekai.mx/register?token={jwt.Token}";
            var emailValidation = new EmailValidationMail(dbUser.Email, link);

            try
            {
                emailValidation.SendEmail();
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponse()
                {
                    Error = e.Message
                });
            }

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
            var dbUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (dbUser != null && dbUser.AuthParamsId != null)
            {
                var auth = new JsonWebTokenAuth(dbUser.Id, dbUser.Email);
                return Ok(new
                {
                    Response = auth.Token
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

            if (dbUser != null)
            {
                dbUser.AuthParams = authParams;
                _context.Update(dbUser);
            }
            else
            {
                dbUser = new UserItem()
                {
                    Id = id,
                    Email = email,
                    AuthParams = authParams,
                };
                _context.Users.Add(dbUser);
            }

            _context.SaveChanges();
            var jwt = new JsonWebTokenAuth(dbUser.Id, dbUser.Email);

            return Ok(new
            {
                Response = jwt.Token
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