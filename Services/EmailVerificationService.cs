using System;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PortfolioTrackerApi.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly ConcurrentDictionary<string, VerificationEntry> _verificationCodes = new();
        private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(5); // 5 minutes

        public async Task SendVerificationCode(string email)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email address.");

            var code = new Random().Next(100000, 999999).ToString();

            var entry = new VerificationEntry
            {
                Code = code,
                CreatedAt = DateTime.UtcNow
            };

            _verificationCodes[email] = entry;

            await SendEmail(email, code);

            Console.WriteLine($"[Email to {email}]: Your verification code for FinTrack is {code}");
        }

        public bool VerifyCode(string email, string code)
        {
            if (_verificationCodes.TryGetValue(email, out var entry))
            {
                if (DateTime.UtcNow - entry.CreatedAt > _codeExpiration)
                {
                    _verificationCodes.TryRemove(email, out _);
                    return false; // expired
                }

                if (entry.Code == code)
                {
                    _verificationCodes.TryRemove(email, out _);
                    return true; // valid
                }
            }
            return false;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task SendEmail(string toEmail, string code)
        {
            var apiKey = "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"; // 🔥 Replace with your real SendGrid API Key
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("fintrackblr@gmail.com", "FinTrack"); // 🔥 Must be verified in SendGrid
            var subject = "Your Verification Code";
            var to = new EmailAddress(toEmail);
            var plainTextContent = $"Your verification code is: {code}";
            var htmlContent = $"<strong>Your verification code is: {code}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {errorBody}");
            }
        }

        private class VerificationEntry
        {
            public string Code { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
