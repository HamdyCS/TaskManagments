using Api.Polices.WorkSpace.WorkSpaceOwner;
using Api.Polices.WorkSpace.WorkSpaceProjectManager;
using Api.Polices.WorkSpace.WorkSpaceUser;

namespace Api.Common.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            //add handlers
            services.AddScoped<IAuthorizationHandler, WorkSpaceOwnerHandler>();
            services.AddScoped<IAuthorizationHandler, WorkSpaceUserHandler>();
            services.AddScoped<IAuthorizationHandler, WorkSpaceProjectManagerHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("WorkSpaceOwner", 
                    policy => policy.Requirements.Add(new WorkSpaceOwnerRequirement()));
                options.AddPolicy("WorkSpaceUser",
                    policy => policy.AddRequirements(new WorkSpaceUserRequirement()));
                options.AddPolicy("WorkSpaceProjectManager",
                    policy => policy.AddRequirements(new WorkSpaceProjectManagerRequirement()));
            });

            return services;
        }
    }
}
