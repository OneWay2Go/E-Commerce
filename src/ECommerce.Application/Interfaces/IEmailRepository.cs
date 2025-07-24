namespace ECommerce.Application.Interfaces;

public interface IEmailRepository
{
    Task<string> GetEmailCodeAsync(string email);
    Task<bool> IsEmailCodeValidAsync(string email, string code);
}
