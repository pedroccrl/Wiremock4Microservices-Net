using System.Net;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.Models;
using WireMock.RequestBuilders;
using WireMock.Types;
using WireMock.Util;
using WiremockMicroservices.Builders;
using WiremockMicroservices.Endpoints;

namespace WiremockMicroservices.Examples.WebApplication.Endpoints;

public class WebhookEndpoint : WiremockEndpoint
{
    public override IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/webhooks");

    public override IWebhook[] Webhooks
        => new IWebhook[]
        {
            new Webhook
            {
                Request = new WebhookRequest
                {
                    Url = "http://localhost:12345/foo1",
                    Method = "post",
                    BodyData = new BodyData
                    {
                        BodyAsString = "OK 1!",
                        DetectedBodyType = BodyType.String
                    },
                    Delay = 1000
                }
            },
            new Webhook
            {
                Request = new WebhookRequest
                {
                    Url = "http://localhost:12345/foo2",
                    Method = "post",
                    BodyData = new BodyData
                    {
                        BodyAsString = "OK 2!",
                        DetectedBodyType = BodyType.String
                    },
                    MinimumRandomDelay = 3000,
                    MaximumRandomDelay = 7000
                }
            }
        };

    public override Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
        => Task.FromResult(
            ResponseMessageBuilder
                .Create()
                .WithStatusCode(HttpStatusCode.Accepted)
                .BuildWithEmptyBody()
            );
}