using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Shinsekai_API.Models;

namespace Shinsekai_API.Authentication
{
    public class AuthService : IAuth
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public AuthService(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string AuthByEmailAndPassword(UserItem user) 
        {
            var jwt = new JsonWebTokenAuth(user.Id, user.Email);
            var passwordService = new PasswordService(Password, user.AuthParams.Salt);

            return passwordService.HashPassword != user.AuthParams.Password ? null : jwt.Token;
        }

        public static string IdentifyUser(IIdentity identity)
        {
            var user = (ClaimsIdentity)identity;
            var id = user.Claims.Where(r => r.Type == ClaimTypes.SerialNumber)
                .Select(c => c.Value)
                .First();
           return id;
        }

        public static bool AuthorizeAdmin(IIdentity identity, List<UserItem> users)
        {
            var id = IdentifyUser(identity);
            var dbAdminUser = users.FirstOrDefault(u => u.Id == id);
            
            if (dbAdminUser is not {Admin: true})
            {
                return true;
            }

            return false;
        }
    }
}