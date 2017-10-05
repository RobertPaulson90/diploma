using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Diploma.BLL.Contracts.Services;

namespace Diploma.BLL.Services
{
    internal sealed class CryptoService : ICryptoService
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

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
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentException(nameof(hashedPassword));
            }

            var hashParts = hashedPassword.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (hashParts.Length != 3)
            {
                throw new FormatException($"The {nameof(hashedPassword)} must match the next format: \'iterations:salt:hash\'.");
            }

            var iterations = hashParts[0];

            if (!int.TryParse(iterations, out var iterationsCount))
            {
                throw new ArgumentException("Hash should contain iterations.", nameof(hashedPassword));
            }

            if (iterationsCount <= 1)
            {
                throw new ArgumentException("The number of iterations cannot be less than 1.", nameof(hashedPassword));
            }

            var originalSalt = hashParts[1];

            if (string.IsNullOrWhiteSpace(originalSalt))
            {
                throw new FormatException("Invalid salt.");
            }

            var originalHash = hashParts[2];
            if (string.IsNullOrWhiteSpace(originalHash))
            {
                throw new FormatException("Invalid hash.");
            }

            var originalSaltBytes = Convert.FromBase64String(originalSalt);

            var originalHashBytes = Convert.FromBase64String(originalHash);

            return VerifyPasswordHashInternal(password, iterationsCount, originalSaltBytes, originalHashBytes);
        }

        private static bool VerifyPasswordHashInternal(string password, int iterations, byte[] originalSalt, IReadOnlyList<byte> originalHash)
        {
            using (var hashTool = new Rfc2898DeriveBytes(password, originalSalt, iterations))
            {
                var hash = hashTool.GetBytes(originalHash.Count);
                var differences = (uint)originalHash.Count ^ (uint)hash.Length;
                for (var position = 0; position < Math.Min(originalHash.Count, hash.Length); position++)
                {
                    differences |= (uint)(originalHash[position] ^ hash[position]);
                }

                var passwordMatches = differences == 0;
                return passwordMatches;
            }
        }
    }
}
