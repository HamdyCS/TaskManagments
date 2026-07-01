using Application.Common.Behaviors;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class DI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assamble = typeof(DI).Assembly;
            if (assamble is null)
                throw new Exception("Assembly not found");

            //add fluent validation
            services.AddValidatorsFromAssembly(assamble);

            //add mapstar
            TypeAdapterConfig.GlobalSettings.Scan(assamble);

            //add MediatR
            services.AddMediatR(opt =>
                opt.RegisterServicesFromAssembly(assamble));

            //add validation behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
