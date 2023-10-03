using Microsoft.Extensions.DependencyInjection;
using Moq;
using WireMock;
using WireMock.Settings;
using WiremockMicroservices.Endpoints;
using WiremockMicroservices.ResponseProviders;

namespace WiremockMicroservices.UnitTests.ResponseProviders;

public class DependencyInjectionEndpointResponseProviderTests
{
    private readonly Mock<IServiceScopeFactory> _scopeFactory = new();
    private readonly Mock<IServiceScope> _scope = new();
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    private readonly Type _endpointType = typeof(FakeEndpoint);

    private readonly DependencyInjectionEndpointResponseProvider _responseProvider;

    public DependencyInjectionEndpointResponseProviderTests()
    {
        _scopeFactory
            .Setup(x => x.CreateScope())
            .Returns(_scope.Object);

        _scope
            .SetupGet(x => x.ServiceProvider)
            .Returns(_serviceProvider.Object);
        
        _responseProvider = new(_scopeFactory.Object, _endpointType);
    }

    [Fact]
    public async Task ProvideResponseAsync_GivenRequest_ShouldExecuteAsExpected()
    {
        var mapping = new Mock<IMapping>();
        var requestMessage = new Mock<IRequestMessage>();
        var settings = new WireMockServerSettings();
        var endpoint = new Mock<IWiremockEndpoint>();

        _serviceProvider
            .Setup(x => x.GetService(_endpointType))
            .Returns(endpoint.Object);

        await _responseProvider.ProvideResponseAsync(mapping.Object, requestMessage.Object, settings);
        
        _scopeFactory.Verify(x => x.CreateScope(), Times.Once);
        _serviceProvider.Verify(x => x.GetService(_endpointType), Times.Once);
        endpoint.Verify(x => x.CallbackHandler(requestMessage.Object), Times.Once);
    }
}