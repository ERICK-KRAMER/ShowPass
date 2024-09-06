namespace ShowPass.Models.EmailService
{
    public record SendEmailRequest(string Recipient, string Subject, string Body);
}