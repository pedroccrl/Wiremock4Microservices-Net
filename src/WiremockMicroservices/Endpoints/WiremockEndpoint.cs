using WireMock;
using WireMock.Matchers.Request;
using WireMock.Models;

namespace WiremockMicroservices.Endpoints;

public abstract class WiremockEndpoint : IWiremockEndpoint
{
    public abstract IRequestMatcher RequestMatcher { get; }
    public virtual IWebhook[] Webhooks => Array.Empty<IWebhook>();

    public abstract Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage);
}