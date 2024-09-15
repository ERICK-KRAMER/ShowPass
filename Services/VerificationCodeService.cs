using System.Collections.Concurrent;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Services
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ConcurrentDictionary<string, (string Code, DateTime ExpirationTime)> _codes = new();
        private readonly Random _random = new();

        public string GenerateCode(string userEmail)
        {
            var code = GenerateRandomCode();
            var expirationTime = DateTime.UtcNow.AddMinutes(2);

            _codes[userEmail] = (code, expirationTime);

            return code;
        }

        public bool ValidateCode(string userEmail, string submittedCode)
        {
            if (_codes.TryGetValue(userEmail, out var storedData))
            {
                var (storedCode, expirationTime) = storedData;

                if (DateTime.UtcNow <= expirationTime && storedCode == submittedCode)
                {
                    _codes.TryRemove(userEmail, out _);
                    return true;
                }
            }
            return false;
        }

        private string GenerateRandomCode()
        {
            return _random.Next(100000, 999999).ToString("D6");
        }
    }
}