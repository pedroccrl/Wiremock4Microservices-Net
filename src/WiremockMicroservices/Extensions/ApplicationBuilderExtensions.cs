using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Matchers.Request;
using WireMock.Models;
using WiremockMicroservices.Endpoints;
using WiremockMicroservices.Services;

namespace WiremockMicroservices.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseWiremockEndpoints(this IApplicationBuilder app)
    {
        var wiremockService = app.ApplicationServices.GetRequiredService<IWireMockService>();

        wiremockService.Start();

        var endpointRequestMatcherType = typeof(IWiremockEndpoint);

        var endpoints = WiremockContext
            .AssembliesEndpoints
            .SelectMany(x => x.GetTypes())
            .Where(t => endpointRequestMatcherType.IsAssignableFrom(t));

        using var scope = app.ApplicationServices.CreateScope();
        
        foreach (var endpointType in endpoints)
        {
            var endpoint = (IWiremockEndpoint)scope.ServiceProvider.GetRequiredService(endpointType);
            
            wiremockService.AddEndpoint(endpoint);
        }
    }
}