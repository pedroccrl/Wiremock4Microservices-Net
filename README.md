# WireMock4Microservices.Net
WireMock4Microservices.Net is a .Net library built on top of [WireMock.Net](https://github.com/WireMock-Net/WireMock.Net). It enhances support for mocked requests by injecting one or more services and extends WireMock's features using the original Callback Response.

## :question: Why 4Microservices? 
It's common behavior for microservices to share contracts, SDKs, and messages among themselves. While WireMock supports webhooks, it cannot send or consume messages from queues or topics. By implementing the `IWiremockEndpoint` (see the "Using" [section](https://github.com/pedroccrl/Wiremock4Microservices-Net#page_facing_up-using)), it becomes possible to inject any service and create the expected behavior for simple or more complex scenarios.

## :page_facing_up: Using
To begin creating mocked endpoints, implement the `IWiremockEndpoint` or inherit from `WiremockEndpoint` to start creating mocked endpoints as follows:
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
Start by defining the request matching criteria within the `RequestMatcher` property. For more details, refer to the WireMock.Net [documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Request-Matching).

### Webhooks Property
The Webhooks property is optional and allows you to configure webhooks. Further information can be found in the WireMock.Net [documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Webhook).

### CallbackHandler Method
The `CallbackHandler` method is responsible for generating the response based on the `RequestMessage`. You can use the original `IRequestMessage` and utilize services to create more complex scenarios. Here are examples of different response types:

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

## Using with ASP.NET Web
To use WireMock4Microservices.Net with ASP.NET Web, you need to perform two steps in your Program or Startup file:

```csharp
var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

var services = builder.Services;

services.AddWiremockEndpoints(configuration, Assembly.GetExecutingAssembly());
```

In the `AddWiremockEndpoints` method, the `Assembly` parameter specifies where your implementations of `IWiremockEndpoint` are located. Through reflection, all implementations will be created using the `WireMockServer` singleton instance. 
The `IConfiguration` is used to get the `WireMockServerSettings`, see the WireMock.Net [documentation](https://github.com/WireMock-Net/WireMock.Net/wiki/Settings) for more information.

Finally, use the `IApplicationBuilder` to call the following method:

```csharp
app.UseWiremockEndpoints();
```

This setup enables WireMock4Microservices.Net within your ASP.NET Web application.