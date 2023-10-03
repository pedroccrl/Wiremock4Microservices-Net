using System.Net;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.Models;
using WireMock.RequestBuilders;
using WiremockMicroservices.Builders;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices.UnitTests;

public class FakeEndpoint : WiremockEndpoint
{
    public override IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/json-object");

    public override IWebhook[] Webhooks => Array.Empty<IWebhook>();

    public override async Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        await Task.Delay(1);

        var response = new ResponseMessageBuilder()
            .WithStatusCode(HttpStatusCode.OK)
            .BuildWithDataAsJson(new
            {
                currentDate = DateTime.Now
            });

        return response;
    }
}