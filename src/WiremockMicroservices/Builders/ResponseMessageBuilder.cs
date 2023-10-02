using System.Net;
using System.Text;
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
        => new();

    public ResponseMessageBuilder WithStatusCode(HttpStatusCode statusCode)
    {
        _response.StatusCode = statusCode;

        return this;
    }

    public ResponseMessageBuilder WithHeader(string key, string value, bool replace = true)
    {
        if (_response.Headers!.ContainsKey(key) && replace)
        {
            _response.Headers[key] = value;
        }
        else
        {
            _response.AddHeader(key, value);
        }

        return this;
    }

    public ResponseMessage BuildWithEmptyBody()
    {
        _response.BodyData = new BodyData();

        return _response;
    }

    public ResponseMessage BuildWithDataAsJson(object data, JsonSerializerSettings jsonSerializerSettings)
    {
        var dataJson = JsonConvert.SerializeObject(data, jsonSerializerSettings);

        _response.BodyData = new BodyData
        {
            BodyAsString = dataJson,
            DetectedBodyType = BodyType.Json
        };
        
        WithHeader("Content-Type", "application/json");

        return _response;
    }

    public ResponseMessage BuildWithDataAsJson(object data)
    {
        _response.BodyData = new BodyData
        {
            BodyAsJson = data,
            DetectedBodyType = BodyType.Json
        };
        
        WithHeader("Content-Type", "application/json");

        return _response;
    }

    public ResponseMessage BuildWithText(string text)
    {
        _response.BodyData = new BodyData
        {
            BodyAsString = text,
            DetectedBodyType = BodyType.String
        };
        
        WithHeader("Content-Type", "text/plain");

        return _response;
    }
    
    public ResponseMessage BuildWithStringAsJson(string json)
    {
        _response.BodyData = new BodyData
        {
            BodyAsString = json,
            DetectedBodyType = BodyType.String,
            Encoding = Encoding.UTF8
        };

        WithHeader("Content-Type", "application/json");

        return _response;
    }
}