namespace ShowPass.Repositories.Interfaces
{
    public interface IPasswordHashService
    {
        public string Hash(string password);
        public bool Verify(string password, string passwordHash);
    }
}