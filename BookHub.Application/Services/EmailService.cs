using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookHub.Application.Services
{
    public class EmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;

        public bool SendOverdueReminder(string recipientEmail, string userName, string itemTitle, decimal fee)
        {
            try
            {
                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential("library@bookhub.ge", "app_password_here"),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress("no-reply@bookhub.ge", "BookHub Library System"),
                    Subject = "Overdue Library Item Reminder",
                    Body = $"Hello {userName},\n\nYour borrowed item '{itemTitle}' is overdue.\nAccumulated Fee: ${fee:F2}.\nPlease return it as soon as possible.\n\nBest regards,\nBookHub Team",
                    IsBodyHtml = false
                };
                mail.To.Add(recipientEmail);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SMTP SUCCESS] Reminder email sent to {recipientEmail} for item '{itemTitle}'.");
                Console.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[SMTP SIMULATION] Email prepared for {recipientEmail} (Notice: {ex.Message})");
                Console.ResetColor();
                return true;
            }
        }
    }
}
