using Api.Polices;
using Api.Polices.WorkSpace.WorkSpaceOwner;
using Api.Polices.WorkSpace.WorkSpaceProjectManager;
using Api.Polices.WorkSpace.WorkSpaceUser;
using System.Security.Claims;
using System.Threading.RateLimiting;

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

        private static string GetClientKey(HttpContext httpContext)
        {
            if(httpContext.User.Identity?.IsAuthenticated == true)
            {
                return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
          
            var ipAddress = httpContext.Connection.RemoteIpAddress;
            var clientId = httpContext.Request.Cookies["Client-Id"];

            return $"{ipAddress}-{clientId}";

        }
        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                ;

                //global rate limit for all requests

                options.AddPolicy(RateLimitingPolicies.GlobalRateLimit, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromSeconds(10),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                //Authentication rate limit
                options.AddPolicy(RateLimitingPolicies.AuthenticationActionsRateLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );


                //login register rate limit
                options.AddPolicy(RateLimitingPolicies.LoginRegisterRateLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromSeconds(10),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );

                //refresh token rate limit
                options.AddPolicy(RateLimitingPolicies.RefreshTokenRateLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );

                //global rate limit for all authenticated get requests
                options.AddPolicy(RateLimitingPolicies.GlobalAuthenticatedGetRateLimit, httpContext =>
                
                   RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );


                //global rate limit for all authenticated write requests

                options.AddPolicy(RateLimitingPolicies.GlobalAuthenticatedWriteRateLimit, httpContext =>
              RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );

                //dashboard rate limit
                options.AddPolicy(RateLimitingPolicies.DashboardRateLimit, httpContext =>
                
                  RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );

                //report rate limit
                options.AddPolicy(RateLimitingPolicies.ReportRateLimit, httpContext =>
               RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );

                //report download rate limit
                options.AddPolicy(RateLimitingPolicies.ReportDownloadRateLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientKey(httpContext),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        })
                );
            });
            return services;
        }
    }
}
