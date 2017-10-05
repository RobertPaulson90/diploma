using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Diploma.BLL.Contracts.Services;
using Diploma.BLL.Properties;

namespace Diploma.BLL.Services
{
    internal sealed class CryptoService : ICryptoService
    {
        internal const string HashDelimiter = ":";

        internal const int HashSize = 16;

        internal const int Iterations = 1000;

        internal const int SaltSize = 16;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations))
            {
                var salt = rfc2898DeriveBytes.Salt;
                var hash = rfc2898DeriveBytes.GetBytes(HashSize);
                var base64Salt = Convert.ToBase64String(salt);
                var base64Hash = Convert.ToBase64String(hash);
                return string.Join(HashDelimiter, Iterations, base64Salt, base64Hash);
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

            var hashParts = hashedPassword.Split(new[] { HashDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            if (hashParts.Length != 3)
            {
                throw new FormatException(string.Format(Resources.Exception_Hash_Has_Wrong_Format, nameof(hashedPassword)));
            }

            var iterations = hashParts[0];

            if (!int.TryParse(iterations, out var iterationsCount))
            {
                throw new ArgumentException(Resources.Exception_Hash_Algorithm_Iterations_Not_Specified, nameof(hashedPassword));
            }

            if (iterationsCount <= 1)
            {
                throw new ArgumentException(Resources.Exception_Hash_Algorithm_Iterations_Is_Less_Than_One, nameof(hashedPassword));
            }

            var originalBase64Salt = hashParts[1];

            if (string.IsNullOrWhiteSpace(originalBase64Salt))
            {
                throw new FormatException(string.Format(Resources.Exception_Hash_Algorithm_Salt_Not_Specified, nameof(hashedPassword)));
            }

            var originalBase64Hash = hashParts[2];
            if (string.IsNullOrWhiteSpace(originalBase64Hash))
            {
                throw new FormatException(string.Format(Resources.Exception_Hash_Algorithm_Hash_Not_Specified, nameof(hashedPassword)));
            }

            var originalSalt = Convert.FromBase64String(originalBase64Salt);

            var originalHash = Convert.FromBase64String(originalBase64Hash);

            return VerifyPasswordHashInternal(password, iterationsCount, originalSalt, originalHash);
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
