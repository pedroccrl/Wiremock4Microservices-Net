using System.Net;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WiremockMicroservices.Builders;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices.Examples.WebApplication.Endpoints;

public class JsonObjectEndpoint : IWiremockEndpoint
{
    public static IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/json-object");

    public async Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        await Task.Delay(1);
        
        var response = new ResponseMessageBuilder()
            .WithStatusCode(HttpStatusCode.OK)
            .BuildWithJsonBody(new
            {
                dataAtual = DateTime.Now
            });

        return response;
    }
}