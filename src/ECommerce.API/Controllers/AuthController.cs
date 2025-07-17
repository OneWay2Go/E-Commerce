using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.DTOs;
using ECommerce.Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(
        IAuthService authService,
        IEmailRepository emailRepository,
        ECommerceDbContext context) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            var response = await authService.RegisterAsync(request);

            if (response.Succeeded)
            {
                await transaction.CommitAsync();
                return Ok(response);
            }
            else
            {
                await transaction.RollbackAsync();
                return BadRequest(response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        //[HttpPost("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail([FromQuery]string email, [FromBody]string code)
        //{
        //    var response = await emailRepository.IsEmailCodeValidAsync(email, code);
        //    if (response)
        //    {
        //        return Ok("Email confirmed successfully!");
        //    }
        //    else
        //    {
        //        return BadRequest("Email is already confirmed or code is invalid.");
        //    }
        //}
    }
}
