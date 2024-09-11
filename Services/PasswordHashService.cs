using DevOne.Security.Cryptography.BCrypt;

namespace ShowPass.Services
{
    public class PasswordHashService
    {
        public string Hash(string password)
        {
            var salt = BCryptHelper.GenerateSalt();

            return BCryptHelper.HashPassword(password, salt);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCryptHelper.CheckPassword(password, passwordHash);
        }
    }
}