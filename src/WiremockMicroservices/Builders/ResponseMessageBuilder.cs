using System.Net;
using Newtonsoft.Json;
using WireMock;
using WireMock.Types;
using WireMock.Util;

namespace WiremockMicroservices.Builders;

public class ResponseMessageBuilder
{
    private readonly ResponseMessage _response = new()
    {
        Headers = new Dictionary<string, WireMockList<string>>()
    };

    public static ResponseMessageBuilder Create()
    {
        return new ResponseMessageBuilder();
    }

    public ResponseMessageBuilder WithStatusCode(HttpStatusCode statusCode)
    {
        _response.StatusCode = statusCode;
        
        return this;
    }

    public ResponseMessageBuilder WithHeader(string key, string value)
    {
        _response.AddHeader(key, value);

        return this;
    }

    public ResponseMessage BuildWithEmptyBody()
    {
        _response.BodyData = new BodyData();

        return _response;
    }

    public ResponseMessage BuildWithJsonBody(object data, JsonSerializerSettings jsonSerializerSettings)
    {
        var dataJson = JsonConvert.SerializeObject(data, jsonSerializerSettings);

        _response.BodyData = new BodyData
        {
            BodyAsString = dataJson,
            DetectedBodyType = BodyType.Json
        };

        return _response;
    }

    public ResponseMessage BuildWithJsonBody(object data)
    {
        _response.BodyData = new BodyData
        {
            BodyAsJson = data,
            DetectedBodyType = BodyType.Json
        };

        return _response;
    }

    public ResponseMessage BuildWithText(string text)
    {
        _response.BodyData = new BodyData
        {
            BodyAsString = text,
            DetectedBodyType = BodyType.String
        };

        return _response;
    }
}