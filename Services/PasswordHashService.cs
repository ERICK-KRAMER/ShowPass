namespace ShowPass.Services
{
    public class PasswordHashService
    {
        public string Hash(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return passwordHash;

        }

        public bool Verify(string password, string passwordHash)
        {
            bool verify = BCrypt.Net.BCrypt.Verify(password, passwordHash);

            return verify;
        }
    }
}