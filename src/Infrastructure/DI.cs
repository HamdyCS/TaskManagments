using Domain.Entities;
using Infrastructure.Persistence;
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
                .AddSignInManager<SignInManager<User>>();

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
