using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Repositories;
using ShowPass.Repositories.Interfaces;
using ShowPass.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShowPassDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<TokenService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSingleton<VerificationCodeService>();

builder.Services.AddScoped<PasswordHashService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


