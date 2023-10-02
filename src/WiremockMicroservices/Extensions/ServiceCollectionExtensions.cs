using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Settings;
using WiremockMicroservices.Endpoints;
using WiremockMicroservices.Services;

namespace WiremockMicroservices.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWiremockEndpoints(this IServiceCollection services,
        IConfiguration configuration, params Assembly[] assemblies)
    {
        services.Configure<WireMockServerSettings>(configuration.GetSection(nameof(WireMockServerSettings)));

        WiremockContext.AssembliesEndpoints = assemblies;

        services
            .Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IWiremockEndpoint>())
                .AsSelf()
                .WithScopedLifetime()
            );

        services.AddSingleton<IWireMockService, WireMockService>();

        return services;
    }
}