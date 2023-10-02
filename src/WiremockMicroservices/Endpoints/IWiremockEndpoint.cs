using WireMock.Matchers.Request;
using WireMock.Models;

namespace WiremockMicroservices.Endpoints;

public interface IWiremockEndpoint : IWiremockEndpointCallbackHandler
{
    IRequestMatcher RequestMatcher { get; }
    IWebhook[] Webhooks { get; }
}
