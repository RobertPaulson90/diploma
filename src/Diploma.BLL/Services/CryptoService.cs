using System;
using System.Security.Cryptography;
using Diploma.BLL.Contracts.Services;

namespace Diploma.BLL.Services
{
    internal sealed class CryptoService : ICryptoService
    {
        public string HashPassword(string password)
        {
            const int HashSize = 16;
            const int Iterations = 1000;
            const int SaltSize = 16;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations))
            {
                var salt = rfc2898DeriveBytes.Salt;
                var hash = rfc2898DeriveBytes.GetBytes(HashSize);
                return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        public bool VerifyPasswordHash(string password, string hashedPassword)
        {
            var hashParts = hashedPassword.Split(':');
            var iterations = int.Parse(hashParts[0]);
            var originalSalt = Convert.FromBase64String(hashParts[1]);
            var originalHash = Convert.FromBase64String(hashParts[2]);
            using (var hashTool = new Rfc2898DeriveBytes(password, originalSalt, iterations))
            {
                var hash = hashTool.GetBytes(originalHash.Length);
                var differences = (uint)originalHash.Length ^ (uint)hash.Length;
                for (var position = 0; position < Math.Min(originalHash.Length, hash.Length); position++)
                {
                    differences |= (uint)(originalHash[position] ^ hash[position]);
                }

                var passwordMatches = differences == 0;
                return passwordMatches;
            }
        }
    }
}
