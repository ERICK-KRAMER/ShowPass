using ShowPass.Models;

namespace ShowPass.Repositories.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user, int SetTimeOut);

        public bool ValidateJwtToken(string token);
    }
}