using System.Net;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WiremockMicroservices.Builders;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices;

public class MicroserviceMockFactory
{
    public string Route { get; set; }
}

public class ControllerMockFactory
{
    public string Route { get; set; }
}

public class EndpointMockFactory
{
    
}

class WiremockEndpoint : IWiremockEndpoint
{
    public static IRequestMatcher RequestMatcher()
        => Request.Create().UsingGet().WithPath("/mock");

    public async Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        await Task.Delay(1000);
        
        var response = new ResponseMessageBuilder()
            .WithStatusCode(HttpStatusCode.Accepted)
            .BuildWithJsonBody(new
            {
                dataAtual = DateTime.Now
            });

        return response;
    }
}