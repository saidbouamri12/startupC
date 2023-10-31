using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Service
{
    public class EmailSender
    {
        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string senderEmail = "1bouamri1996@gmail.com";
        private readonly string senderPassword = "uqakqmcwnsnlmgwb";

        public bool SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage(senderEmail, recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = "<html> <body>"+body+"</body></html>";
                mailMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Timeout = 20000; // Adjust this timeout as needed

                smtpClient.Send(mailMessage);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Sending Email: "+ex.Message);
                return false;
            }
        }
    }
}
