namespace ShowPass.Repositories.Interfaces
{
    public interface IVerificationCodeService
    {
        public string GenerateCode(string userEmail);
        public bool ValidateCode(string userEmail, string submitedCode);
    }
}