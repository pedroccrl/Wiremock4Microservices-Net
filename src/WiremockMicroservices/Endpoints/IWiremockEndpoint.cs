using WireMock.Matchers.Request;

namespace WiremockMicroservices.Endpoints;

public interface IWiremockEndpoint : IWiremockEndpointCallbackHandler
{
    static abstract IRequestMatcher RequestMatcher { get; }
}