using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Domain.Entities;
using Infrastructure.BackgroundServices;
using Infrastructure.common.channels;
using Infrastructure.Common.Channels;
using Infrastructure.Common.Options;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("SqlServer");

            //add dbcontext
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(sqlConnectionString));

            //add Identity
            services.AddCustomIdentity(configuration);

            //add redis cache
            services.AddRedisCache(configuration);

            //add Mail Options
            services.AddMailOptions(configuration);

            //add jwt
            services.AddJwt(configuration);

            //add queues
            services.AddQueues(configuration);

            //repositories
            services.AddRepositories(configuration);

            //services
            services.AddServices(configuration);

            //background services
            services.AddBackgroundServices(configuration);

            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection();

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddMailOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var emailOptions = configuration.GetSection("Mail").Get<MailOptions>();

            if (emailOptions is null)
                throw new Exception("MailOptions is null");
            else
                services.AddSingleton(emailOptions);


            return services;
        }

        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            if (jwtOptions is null)
                throw new Exception("JwtOptions is null");
            else
                services.AddSingleton(jwtOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.SigningKey)),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    TokenDecryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.EncryptionKey))
                };

                //read token from cookie
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddQueues(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfirmationEmailQueue, ConfirmationEmailQueue>();
            services.AddSingleton<IOtpEmailQueue, OtpEmailQueue>();
            services.AddSingleton<IResetPasswordEmailQueue, ResetPasswordEmailQueue>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IOtpService, OtpService>();

            return services;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ConfirmationEmailBgService>();
            services.AddHostedService<RemoveUnConfirmedUsersBgService>();
            services.AddHostedService<OtpEmailBgService>();
            services.AddHostedService<ResetPasswordEmailBgService>();

            //add redis cache
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = configuration.GetConnectionString("Redis");
                opt.InstanceName = "RedisCache";
            });

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            //add redis cache
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = configuration.GetConnectionString("Redis");
                opt.InstanceName = "RedisCache";
            });

            return services;
        }
    }
}
