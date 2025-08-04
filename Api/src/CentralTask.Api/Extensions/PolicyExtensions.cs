using CentralTask.Api.Authorization;
using CentralTask.Domain.Enums;

namespace CentralTask.Api.Extensions;
public static class PolicyExtensions
{
    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(Policies.Admin, policy =>
            {
                policy.RequireRole(EnumNivel.Admin.ToString());
            });
        });

        return services;
    }
}
