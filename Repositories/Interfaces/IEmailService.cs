namespace ShowPass.Repositories.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string recipient, string subject, string body, bool isHtml = true);
    }
}