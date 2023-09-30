using WireMock;

namespace WiremockMicroservices.Endpoints;

public interface IWiremockEndpointCallbackHandler
{
    Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage);
}