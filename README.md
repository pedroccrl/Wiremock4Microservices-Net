# WireMock4Microservices.Net
A WireMock .Net library based on [WireMock.Net](https://github.com/WireMock-Net/WireMock.Net) which adds support for mocked requests by injecting one or more services and extends the features of WireMock using the original Callback Response.

## :question: Why 4Microservices? 
Its a common behaviour of microservices to share the contracts, SDKs and messages between them. The WireMock supports Webhooks but cant send or consume messages of queues or topics. By implementing the IWiremockEndpoint (see the Using section), its possible to inject any service and create the expected behaviour.

## :page_facing_up: Using
Implement the `IWiremockEndpoint` or inherit from `WiremockEndpoint` to start creating mocked endpoints. Looks like this:
```csharp
class MyFakeEndpoint : IWiremockEndpoint
{
    public IRequestMatcher RequestMatcher { get; }
    public IWebhook[] Webhooks { get; }

    public Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        throw new NotImplementedException();
    }
}
```

### RequestMatcher Property
First create the Request Matching on property `RequestMatcher`. See the WireMock.Net [documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Request-Matching) for more details.

### Webhooks Property
Optional property to configure the Webhooks. See the WireMock.Net [documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Webhook) for more details.

### CallbackHandler Method
The method responsible for generate the response based on RequestMessage, here you can use the original IRequestMessage and uses services to create more complex scenarios.

#### Single text
```csharp
public class GetHealth : WiremockEndpoint
{
    public override IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/health");

    public override Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        return Task.FromResult(
            ResponseMessageBuilder
                .Create()
                .WithStatusCode(HttpStatusCode.OK)
                .BuildWithText("Healthy")
        );
    }
}
```

#### Json String
```csharp
public class PostOrdersCheckoutWillReturnAccepted : WiremockEndpoint
{
    public override IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/orders/checkout");

    public override Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        return Task.FromResult(
            ResponseMessageBuilder
                .Create()
                .WithStatusCode(HttpStatusCode.Accepted)
                .BuildWithStringAsJson("""
                                       {
                                         "orderId: "xpto",
                                         "status": "Processing"
                                       }
                                       """));
    }
}
```

#### Json Data
```csharp
public class PostOrdersCheckoutWillReturnAccepted : WiremockEndpoint
{
    public override IRequestMatcher RequestMatcher
        => Request.Create().UsingGet().WithPath("/orders/checkout");

    public override Task<ResponseMessage> CallbackHandler(IRequestMessage requestMessage)
    {
        return Task.FromResult(
            ResponseMessageBuilder
                .Create()
                .WithStatusCode(HttpStatusCode.Accepted)
                .BuildWithDataAsJson(new() { orderId = "xpto", status = "Processing" }));
    }
}
```

## Using on ASP.NET Web
All you need to is call two methods on your Program or Startup file:

```csharp
var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

var services = builder.Services;

services.AddWiremockEndpoints(configuration, Assembly.GetExecutingAssembly());
```

The Assembly param on `AddWiremockEndpoints` method is where your implementations of `IWiremockEndpoint` is. By reflection, all implementations will be created using the `WireMockServer` singleton instance.
And finally, using the `IApplicationBuilder`, call the method above:
```csharp
app.UseWiremockEndpoints();
```