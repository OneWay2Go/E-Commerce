using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Infrastructure.Services;

public class EmailService(IEmailRepository emailRepository, IConfiguration config)
{
    public async Task<bool> SendEmailAsync(string to)
    {
        var from = config["EmailSettings:From"];
        var smtpServer = config["EmailSettings:SmtpServer"];
        var port = int.Parse(config["EmailSettings:Port"]);
        var username = config["EmailSettings:Username"];
        var password = config["EmailSettings:Password"];

        using var client = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var code = await emailRepository.GetEmailCodeAsync(to);
        if(string.IsNullOrEmpty(code))
        {
            return false;
        }

        var message = new MailMessage(from, to)
        {
            Subject = "Your verification code for ECommerceApp",
            Body = code.ToString(),
            IsBodyHtml = true
        };

        try
        {
            await client.SendMailAsync(message);
            return true;
        }
        catch(Exception ex)
        {
            Log.Logger.Error(ex, "Error sending code to: {email}", to);
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
