using System.Net;
using System.Net.Mail;

namespace ShowPass.Services
{
    public static class EmailService
    {
        private static readonly string SmtpServer = "sandbox.smtp.mailtrap.io";
        private static readonly string Username = "944906f28912c0";
        private static readonly string Password = "35065fcbecb150";
        private static readonly string FromEmail = "noreply@showpass.com";
        private static readonly int Port = 2525;

        public static void SendEmail(string recipient, string subject, string body, bool isHtml = true)
        {
            try
            {
                var client = new SmtpClient(SmtpServer, Port)
                {
                    Credentials = new NetworkCredential(Username, Password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(FromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                mailMessage.To.Add(recipient);

                client.Send(mailMessage);
                Console.WriteLine("Email enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
            }
        }
    }
}