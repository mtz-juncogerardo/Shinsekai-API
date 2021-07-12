using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shinsekai_API.Config;
using Shinsekai_API.Models;

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

        public JsonWebTokenAuth(string tokenId, string email, IConfiguration configuration, bool smallExpiration = false)
        {
            _tokenId = tokenId;
            _email = email;
            _smallExpiration = smallExpiration;
            _password = string.Empty;
            _salt = string.Empty;
            Token = GenerateJwtToken(configuration);
        }

        public JsonWebTokenAuth(UserItem user, IConfiguration configuration, bool smallExpiration = false)
        {
            _tokenId = user.Id;
            _email = user.Email;
            Token = GenerateJwtToken(user, configuration);
        }

        public JsonWebTokenAuth(string tokenId, string email, string password, string salt, IConfiguration configuration)
        {
            _tokenId = tokenId;
            _email = email;
            _password = password;
            _salt = salt;
            _smallExpiration = false;
            Token = GenerateJwtToken(configuration);
        }
        private string GenerateJwtToken(IConfiguration config)
        {
            var configuration = new ApiConfiguration(config);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecretKey));
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
                expires: _smallExpiration ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddDays(3),
                signingCredentials: loginCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        private string GenerateJwtToken(UserItem user, IConfiguration config)
        {
            var configuration = new ApiConfiguration(config);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecretKey));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
            var tokenOptions = new JwtSecurityToken(
                "https://localhost:5001",
                "https://localhost:5001",
                new List<Claim>
                {
                    new(ClaimTypes.SerialNumber, _tokenId),
                    new(ClaimTypes.Email, _email),
                    new (ClaimTypes.Locality, user.Address),
                    new (ClaimTypes.Country, user.City),
                    new (ClaimTypes.PostalCode, user.PostalCode),
                    new (ClaimTypes.HomePhone, user.Phone),
                    new (ClaimTypes.Name, user.Name)
                },
                expires: _smallExpiration ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddDays(3),
                signingCredentials: loginCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString; 
        }
    }
}

