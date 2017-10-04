namespace Diploma.BLL.Tests
{
    using Diploma.BLL.Services;
    using NUnit.Framework;

    public class CryptoServiceTests
    {
        [TestCase("abc")]
        public void VerifyPasswordHash(string password)
        {
            var service = new CryptoService();
            var hash = service.HashPassword(password);
            Assert.AreEqual(true, service.VerifyPasswordHash(password, hash));
        }
    }
}
