using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
            };
            opt.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers["Token-expired"] = "true";
                    }

                    return Task.CompletedTask;
                }
            };
        }).AddJwtBearer("IgnoreTokenExpirationScheme", opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, //by who
                ValidateAudience = true, //for whom
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidIssuer = "https://localhost:5001", //should come from configuration
                ValidAudience = "https://localhost:5001", //should come from configuration
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}