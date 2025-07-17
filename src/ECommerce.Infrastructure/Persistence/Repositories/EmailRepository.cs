using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class EmailRepository(ECommerceDbContext context, IUserRepository userRepository) : IEmailRepository
{
    public async Task<string> GetEmailCodeAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return "";
        }
        return user.ToString(); // user.Code.ToString()
    }

    public async Task<bool> IsEmailCodeValidAsync(string email, string code)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

        //if (user == null || user.IsEmailConfirmed)
        //{
        //    return false;
        //}
        
        //if (user.Code == code)
        //{
        //    user.IsEmailConfirmed = true;
        //}

        userRepository.Update(user);
        await context.SaveChangesAsync();

        return true;
    }
}
