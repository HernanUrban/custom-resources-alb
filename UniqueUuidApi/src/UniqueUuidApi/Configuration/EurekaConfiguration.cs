using Steeltoe.Discovery.Client;
using Microsoft.Extensions.DependencyInjection;

namespace UniqueUuidApi.Configuration;

public static class EurekaConfiguration
{
    public static IServiceCollection AddEureka(this IServiceCollection services)
    {
        services.AddDiscoveryClient();
        return services;
    }
}