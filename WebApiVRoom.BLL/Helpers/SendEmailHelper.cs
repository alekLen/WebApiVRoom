using MimeKit;
using MailKit.Net.Smtp;

namespace WebApiVRoom.BLL.Helpers
{
    public static class SendEmailHelper
    {
        public static void SendEmailMessage(string userName, string userEmail, string text)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("VRoom Team", "vroomteamit@gmail.com"));
                message.To.Add(new MailboxAddress(userName, userEmail));
                message.Subject = "Welcome to VRoom";

                var htmlContent = $@"
            <html>
            <body>
                <h1 style='font-size:24px; color:blue;'>Hello, {userName}!</h1>
                <p style='font-size:16px;'>{text}</p>
                <p>Yours respectfully,<br>
                <span style=' color:green;'>Team VRoom</span></p>
            </body>
            </html>";

                message.Body = new TextPart("html")
                {
                    Text = htmlContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("vroomteamit@gmail.com", "mrmb yara ecfw loqt");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            }
        }
        public static void SendHelpMessage(string userName, string userEmail, string text)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(userName, userEmail));
                message.To.Add(new MailboxAddress("VRoom Team", "vroomteamit@gmail.com"));
                message.Subject = "Help_Message";

                var htmlContent = $@"
            <html>
            <body>
                <span>Help message from</span>
                <h1 > {userName}.</h1>
                <p style='font-size:16px;'>{text}</p>
            </body>
            </html>";

                message.Body = new TextPart("html")
                {
                    Text = htmlContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("vroomteamit@gmail.com", "mrmb yara ecfw loqt");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            }
        }

    }
}
