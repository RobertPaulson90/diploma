using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Diploma.WebAPI.Infrastructure.Security
{
    internal sealed class PasswordHasher : IPasswordHasher
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

            using (var hashTool = new Rfc2898DeriveBytes(password, SaltSize, Iterations))
            {
                var salt = hashTool.Salt;
                var hash = hashTool.GetBytes(HashSize);
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

            return VerifyPasswordHashInternal(password, hashedPassword);
        }

        private static uint SlowEquals(IReadOnlyList<byte> first, IReadOnlyList<byte> second)
        {
            var differences = (uint)first.Count ^ (uint)second.Count;
            var length = Math.Min(first.Count, second.Count);
            for (var position = 0; position < length; position++)
            {
                differences |= (uint)(first[position] ^ second[position]);
            }

            return differences;
        }

        private static bool VerifyPasswordHashInternal(string password, string hashedPassword)
        {
            var hashParts = hashedPassword.Split(new[] { HashDelimiter }, StringSplitOptions.RemoveEmptyEntries);

            if (hashParts.Length != 3)
            {
                throw new FormatException(string.Format("Exception_Hash_Has_Wrong_Format", nameof(hashedPassword)));
            }

            if (!int.TryParse(hashParts[0], out var iterationsCount))
            {
                throw new ArgumentException("Exception_Hash_Algorithm_Iterations_Not_Specified", nameof(hashedPassword));
            }

            if (iterationsCount <= 1)
            {
                throw new ArgumentException("Exception_Hash_Algorithm_Iterations_Is_Less_Than_One", nameof(hashedPassword));
            }

            return VerifyPasswordHashInternal(password, hashParts[1], hashParts[2], iterationsCount);
        }

        private static bool VerifyPasswordHashInternal(string password, string salt, string hash, int iterations)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new FormatException(string.Format("Exception_Hash_Algorithm_Salt_Not_Specified", nameof(salt)));
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new FormatException(string.Format("Exception_Hash_Algorithm_Hash_Not_Specified", nameof(hash)));
            }

            var originalSalt = Convert.FromBase64String(salt);

            var originalHash = Convert.FromBase64String(hash);

            return VerifyPasswordHashInternal(password, originalHash, originalSalt, iterations);
        }

        private static bool VerifyPasswordHashInternal(string password, IReadOnlyList<byte> originalHash, byte[] salt, int iterations)
        {
            using (var hashTool = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                var hash = hashTool.GetBytes(originalHash.Count);
                var passwordMatches = SlowEquals(originalHash, hash) == 0;
                return passwordMatches;
            }
        }
    }
}