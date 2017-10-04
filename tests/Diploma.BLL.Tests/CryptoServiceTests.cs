using System;
using Diploma.BLL.Services;
using NUnit.Framework;

namespace Diploma.BLL.Tests
{
    [TestFixture]
    public class CryptoServiceTests
    {
        private CryptoService _cryptoService;

        [TestCase("1q2w3e")]
        public void HashPassword_Should_Return_Different_Hashes_For_Same_Password(string password)
        {
            var hash1 = _cryptoService.HashPassword(password);
            var hash2 = _cryptoService.HashPassword(password);

            Assert.AreNotEqual(hash1, hash2);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void HashPassword_Should_Throw_ArgumentNullException_When_Password_Is_Null_Or_WhiteSpace(string password)
        {
            Assert.Throws<ArgumentNullException>(() => _cryptoService.HashPassword(password));
        }

        [OneTimeSetUp]
        public void Init()
        {
            _cryptoService = new CryptoService();
        }

        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA==: ")]
        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q=%")]
        public void VerifyPasswordHash_Should_Throw_ArgumentException_When_Hash_Is_Not_Valid_Base64_String(string password, string passwordHash)
        {
            Assert.Throws<FormatException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", "ddsd:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        [TestCase("1q2w3e", " :grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        public void VerifyPasswordHash_Should_Throw_ArgumentException_When_Not_Contains_Iterations(string password, string passwordHash)
        {
            Assert.Throws<ArgumentException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", "0:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        [TestCase("1q2w3e", "-1:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        public void VerifyPasswordHash_Should_Throw_ArgumentException_When_Number_Of_Iterations_Less_Than_One(string password, string passwordHash)
        {
            Assert.Throws<ArgumentException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", "1000")]
        [TestCase("1q2w3e", "1000:")]
        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA==:")]
        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==:1000")]
        public void VerifyPasswordHash_Should_Throw_ArgumentException_When_PasswordHash_Has_Wrong_Format(string password, string passwordHash)
        {
            Assert.Throws<FormatException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", "1000: :KioCDLN6rAHjc2KiEc/q5Q==")]
        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA=%:KioCDLN6rAHjc2KiEc/q5Q==")]
        public void VerifyPasswordHash_Should_Throw_ArgumentException_When_Salt_Is_Not_Valid_Base64_String(string password, string passwordHash)
        {
            Assert.Throws<FormatException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase(null, "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        [TestCase("", "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        [TestCase(" ", "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        public void VerifyPasswordHash_Should_Throw_ArgumentNullException_When_Password_Is_Null_Or_WhiteSpace(string password, string passwordHash)
        {
            Assert.Throws<ArgumentNullException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", null)]
        [TestCase("1q2w3e", "")]
        [TestCase("1q2w3e", " ")]
        public void VerifyPasswordHash_Should_Throw_ArgumentNullException_When_PasswordHash_Is_Null_Or_WhiteSpace(
            string password,
            string passwordHash)
        {
            Assert.Throws<ArgumentNullException>(() => _cryptoService.VerifyPasswordHash(password, passwordHash));
        }

        [TestCase("1q2w3e", "1000:grNq68etgzXX6tXDPaCmdA==:KioCDLN6rAHjc2KiEc/q5Q==")]
        public void VerifyPasswordHash_Should_Verify_Password_Against_Valid_Hash(string password, string passwordHash)
        {
            var isHashMatchPassword = _cryptoService.VerifyPasswordHash(password, passwordHash);

            Assert.True(isHashMatchPassword);
        }
    }
}
