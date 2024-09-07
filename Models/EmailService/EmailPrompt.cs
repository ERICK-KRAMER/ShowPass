namespace ShowPass.Models.EmailService
{
    public class EmailPrompt
    {
        public EmailMessage GenerateAccountCreationEmail(string userName, string email)
        {
            return new EmailMessage
            {
                Subject = "Bem-vindo(a) à nossa plataforma!",
                Body = $@"
                <html>
                <body>
                    <h3>Olá <strong>{userName}</strong>,</h3>

                    <p>Obrigado por criar uma conta conosco! Estamos felizes em tê-lo(a) como parte da nossa comunidade.</p>

                    <p>Por favor, confirme o seu email clicando no link abaixo:</p>
                    <p><a href='[LINK]'>CONFIRMAR EMAIL</a></p>

                    <p>Caso você não tenha criado esta conta, por favor, desconsidere este email.</p>

                    <p>Atenciosamente,<br>
                    <i>noreply@showpass.com</i></p>
                </body>
                </html>"
            };
        }

        public EmailMessage GeneratePasswordRecoveryEmail(string userName, string recoveryCode)
        {
            return new EmailMessage
            {
                Subject = "Recuperação de Senha",
                Body = $@"
                <html>
                <body>
                    <h3>Olá <strong>{userName}</strong>,</h3>

                    <p>Recebemos uma solicitação para redefinir sua senha. Se você fez essa solicitação, clique no link abaixo para redefinir sua senha:</p>
                    <p><strong>{recoveryCode}</strong></p>

                    <p>Se você não solicitou a redefinição de senha, desconsidere este email.</p>

                    <p>Atenciosamente,<br>
                    <i>noreply@showpass.com</i></p>
                </body>
                </html>"
            };
        }

        public EmailMessage GeneratePurchaseConfirmationEmail(string userName, string orderNumber, decimal totalPrice)
        {
            return new EmailMessage
            {
                Subject = $"Confirmação de Compra - Pedido {orderNumber}",
                Body = $@"
                <html>
                <body>
                    <h3>Olá <strong>{userName}</strong>,</h3>

                    <p>Obrigado por sua compra! Seu pedido número <strong>{orderNumber}</strong> foi confirmado.</p>
                    <p>O valor total da compra foi de <strong>{totalPrice:C}</strong>.</p>

                    <p>Você pode acompanhar o status do seu pedido acessando sua conta.</p>

                    <p>Atenciosamente,<br>
                    <i>noreply@showpass.com</i></p>
                </body>
                </html>"
            };
        }
    }
}
