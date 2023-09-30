using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WireMock.Matchers.Request;
using WireMock.Server;
using WireMock.Settings;
using WiremockMicroservices.Logging;
using WiremockMicroservices.ResponseProviders;

namespace WiremockMicroservices.Services;

internal class WireMockService : IWireMockService, IDisposable
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WireMockServerSettings _settings;
    private WireMockServer _server;

    public WireMockService(
        ILogger<WireMockService> logger,
        IOptions<WireMockServerSettings> settings,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        settings.Value.Logger = new WiremockLogger(logger);

        _settings = settings.Value;
    }

    public void Start()
    {
        _logger.LogInformation("WireMock.Net server starting");

        _server = WireMockServer.Start(_settings);

        _logger.LogInformation($"WireMock.Net server settings {JsonConvert.SerializeObject(_settings)}");
    }

    public void AddEndpoint(IRequestMatcher requestMatcher, Type callbackHandlerEndpointType)
    {
        _server
            .Given(requestMatcher)
            .RespondWith(new DependencyInjectionEndpointResponseProvider(_scopeFactory, callbackHandlerEndpointType));
    }
    
    public void Dispose()
    {
        _logger.LogInformation("WireMock.Net server stopping");
        _server.Stop();
        _server.Dispose();
    }
}