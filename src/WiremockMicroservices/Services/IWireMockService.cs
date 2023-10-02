using WireMock.Matchers.Request;
using WireMock.Models;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices.Services;

internal interface IWireMockService
{
    void Start();
    void AddEndpoint(IWiremockEndpoint wiremockEndpoint);
}