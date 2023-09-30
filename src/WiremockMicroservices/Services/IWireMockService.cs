using WireMock.Matchers.Request;

namespace WiremockMicroservices.Services;

internal interface IWireMockService
{
    void Start();
    void AddEndpoint(IRequestMatcher requestMatcher, Type callbackHandlerEndpointType);
}