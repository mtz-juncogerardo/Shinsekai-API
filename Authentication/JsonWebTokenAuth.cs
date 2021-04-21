using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shinsekai_API.Config;

namespace Shinsekai_API.Authentication
{
    public class JsonWebTokenAuth
    {
        public string Token { get; }
        private readonly string _tokenId;
        private readonly string _email;
        private readonly string _password;
        private readonly string _salt;
        private readonly bool _smallExpiration;

        public JsonWebTokenAuth(string tokenId, string email, bool smallExpiration = false)
        {
            _tokenId = tokenId;
            _email = email;
            _smallExpiration = smallExpiration;
            _password = string.Empty;
            _salt = string.Empty;
            Token = GenerateJwtToken();
        }

        public JsonWebTokenAuth(string tokenId, string email, string password, string salt)
        {
            _tokenId = tokenId;
            _email = email;
            _password = password;
            _salt = salt;
            _smallExpiration = false;
            Token = GenerateJwtToken();
        }
        private string GenerateJwtToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(APIConfiguration.JwtSecretKey));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
            var tokenOptions = new JwtSecurityToken(
                "https://localhost:5001",
                "https://localhost:5001",
                new List<Claim>
                {
                    new(ClaimTypes.SerialNumber, _tokenId),
                    new(ClaimTypes.Email, _email),
                    new (ClaimTypes.Hash, _password),
                    new (ClaimTypes.Authentication, _salt)
                },
                expires: _smallExpiration ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddDays(1),
                signingCredentials: loginCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}

