using Microsoft.Extensions.DependencyInjection;
using WireMock;
using WireMock.ResponseProviders;
using WireMock.Settings;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices.ResponseProviders;

public class DependencyInjectionEndpointResponseProvider : IResponseProvider
{
    private readonly Type _endpointType;
    private readonly IServiceScopeFactory _scopeFactory;

    public DependencyInjectionEndpointResponseProvider(IServiceScopeFactory scopeFactory, Type endpointType)
    {
        _scopeFactory = scopeFactory;
        _endpointType = endpointType;
    }

    public async Task<(IResponseMessage Message, IMapping? Mapping)> ProvideResponseAsync(IMapping mapping,
        IRequestMessage requestMessage, WireMockServerSettings settings)
    {
        using var scope = _scopeFactory.CreateScope();

        var endpoint = scope.ServiceProvider.GetRequiredService(_endpointType);

        var endpointCallback = endpoint as IWiremockEndpointCallbackHandler;

        var response = await endpointCallback.CallbackHandler(requestMessage);

        return (response, mapping);
    }
}