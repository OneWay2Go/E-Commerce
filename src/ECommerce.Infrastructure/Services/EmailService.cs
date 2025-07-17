using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Infrastructure.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IEmailRepository _email;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailOptions, IEmailRepository email, ILogger<EmailService> logger)
    {
        _emailSettings = emailOptions.Value;
        _email = email;
        _logger = logger;
    }
    public async Task<bool> SendEmailAsync(string email)
    {
        try
        {
            var code = await _email.GetEmailCodeAsync(email);
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient(_emailSettings.SmtpHost);

            mail.From = new MailAddress(_emailSettings.From);
            mail.To.Add(email);
            mail.Subject = "Your verification Code :)";
            mail.Body = GenerateBody(code);
            mail.IsBodyHtml = true;

            smtp.Port = _emailSettings.SmtpPort;
            smtp.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.SmtpPass);
            smtp.EnableSsl = true;

            smtp.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: ", ex.Message);
            return false;
        }
    }
    private string GenerateBody(string otp)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html><body style='font-family:sans-serif;'>");
        sb.AppendLine("<h3>Welcome to E-CommerceApp!</h3>");
        sb.AppendLine("<p>Your one-time verification code is:</p>");
        sb.AppendLine($"<div style='font-size: 24px; font-weight: bold; margin: 20px 0;'>{otp}</div>");
        sb.AppendLine("<p>Please do not share this code with anyone.</p>");
        sb.AppendLine("<p>If you did not request this, please ignore.</p>");
        sb.AppendLine("<br/><small>&copy; 2025 E-CommerceAppp</small>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}
