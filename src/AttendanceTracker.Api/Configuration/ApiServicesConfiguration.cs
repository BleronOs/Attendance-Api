using AttendanceTracker.Api.Interfaces;
using AttendanceTracker.Api.Services;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using AttendanceTracker.Api.Middlewares;
using System.Reflection;

namespace AttendanceTracker.Api.Configuration;

public static class ApiServicesConfiguration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenConfiguration>(configuration.GetSection("TokenConfiguration"));

        var tokenConfiguration =
            configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
        services.AddSingleton<ITokenService>(tokenService => new JwtTokenService(tokenConfiguration));

        //var tokenConfiguration = configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
        //services.Configure<TokenConfiguration>(configuration.GetSection("TokenConfiguration"));

        var emailConfirmation = configuration.GetSection("EmailConfirmation").Get<EmailConfirmation>();
        services.AddSingleton(serviceProvider => emailConfirmation);

        var passwordReset = configuration.GetSection("PasswordReset").Get<PasswordReset>();
        services.AddSingleton(ServiceProvider => passwordReset);

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            loggingBuilder.AddNLog(configuration);
        });


        var info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Attendance Tracker Api Documentation",
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret))
                };
            });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", info);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Description = "Please insert JWT token into field",
                Name = "Authorization" 
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            //c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        services.AddAuthorization();

        return services;
    }
}