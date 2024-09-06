using System.Net;
using System.Net.Mail;

namespace ShowPass.Services
{
    public static class EmailService
    {
        private static readonly string Sender = "YOUR_EMAIL";
        private static readonly string Password = "YOUR_PASSWORD";
        private static readonly string Username = "YOUR_USERNAME";
        private static readonly string SmptServer = "SMPT_SERVER";
        private static readonly int PortSMTP = 0; // PORT_SMTP  

        public static void SendEmail(string recipient, string subject, string body)
        {
            MailMessage message = new MailMessage(Sender, recipient, subject, body);

            SmtpClient clienteSmtp = new SmtpClient(SmptServer, PortSMTP)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true
            };

            try
            {
                clienteSmtp.Send(message);
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
            }
        }
    }
}