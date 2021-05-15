using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Shinsekai_API.Authentication
{
    public class PasswordService
    {
        private readonly string _plainPassword;
        public string Salt;
        public string HashPassword;
        
        public PasswordService(string plainPassword)
        {
            _plainPassword = plainPassword;
            Salt = GetSaltQuery();
            HashPassword = GetHashPassword();
        }

        public PasswordService(string plainPassword, string salt)
        {
            _plainPassword = plainPassword;
            Salt = salt;
            HashPassword = GetHashPassword();
        }
        
        public bool ValidatePassword()
        {
            return _plainPassword != null &&
                   _plainPassword.Length > 8 &&
                   _plainPassword.Any(char.IsUpper) &&
                   _plainPassword.Any(char.IsNumber) &&
                   _plainPassword.Any(char.IsPunctuation);

        }

        private string GetHashPassword()
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var strongPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                _plainPassword,
                Encoding.ASCII.GetBytes(Salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return strongPassword;
        }

        private static string GetSaltQuery()
        {
            var byteSalt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteSalt);
            }
            var stringSalt = Convert.ToBase64String(byteSalt);

            return stringSalt;
        }
    }
}
