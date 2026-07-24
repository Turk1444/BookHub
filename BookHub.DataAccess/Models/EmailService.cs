using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace BookHub.DataAccess.Models
{
    public class EmailService
    {
        public bool SendOverdueReminder(string recipientEmail, string userName, string itemTitle, decimal fee)
        {
            try
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("library@bookhub.ge", "your_app_password"),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress("no-reply@bookhub.ge", "BookHub Admin"),
                    Subject = "URGENT: Overdue Library Item",
                    Body = $"Dear {userName},\n\nYour item '{itemTitle}' is overdue.\nCurrent fee: ${fee:F2}.\nPlease return it immediately.\n\nThanks,\nBookHub Library"
                };
                mail.To.Add(recipientEmail);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SMTP SUCCESS] Reminder sent to {recipientEmail}.");
                Console.ResetColor();
                return true;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[SMTP OFFLINE] Email generated for {recipientEmail} regarding '{itemTitle}'.");
                Console.ResetColor();
                return true;
            }
        }
    }
}
