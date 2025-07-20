using ECommerce.API.Extensions;
using ECommerce.Application.Extensions;
using ECommerce.Application.Models;
using ECommerce.Infrastructure.Auth.Seeders;
using ECommerce.Infrastructure.Extensions;
using ECommerce.Infrastructure.Persistence.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add support to logging with Serilog
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

// Add services to the container.
builder.Services.AddApi(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JwtOptions"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// use PermissionSeeder to seed permissions with ScopeProvider
using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var dbContext = serviceProvider.GetRequiredService<ECommerceDbContext>();
    var permissionSeeder = new PermissionSeeder(dbContext);
    await permissionSeeder.SeedAsync();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
