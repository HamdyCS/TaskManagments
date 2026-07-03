using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Domain.Entities;
using Infrastructure.BackgroundServices;
using Infrastructure.common.channels;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            //add options
            var emailOptions = configuration.GetSection("Mail").Get<MailOptions>();

            if (emailOptions is null)
                throw new Exception("MailOptions is null");
            else
                services.AddSingleton(emailOptions);

            //add queues
            services.AddSingleton<IConfirmationEmailQueue, ConfirmationEmailQueue>();

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //services
            services.AddTransient<IMailService, MailService>();

            //background services
            services.AddHostedService<ConfirmationEmailBackgroundService>();

            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
