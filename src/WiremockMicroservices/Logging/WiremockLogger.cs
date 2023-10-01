using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WireMock.Admin.Requests;
using WireMock.Logging;

namespace WiremockMicroservices.Logging;

internal class WiremockLogger : IWireMockLogger
{
    private readonly ILogger _logger;

    public WiremockLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void Debug(string formatString, params object[] args)
    {
        _logger.LogDebug(formatString, args);
    }

    public void Info(string formatString, params object[] args)
    {
        _logger.LogInformation(formatString, args);
    }

    public void Warn(string formatString, params object[] args)
    {
        _logger.LogWarning(formatString, args);
    }

    public void Error(string formatString, params object[] args)
    {
        _logger.LogError(formatString, args);
    }

    public void DebugRequestResponse(LogEntryModel logEntryModel, bool isAdminrequest)
    {
        var message = JsonConvert.SerializeObject(logEntryModel, Formatting.Indented);

        _logger.LogDebug("Admin[{0}] {1}", isAdminrequest, message);
    }

    public void Error(string formatString, Exception exception)
    {
        _logger.LogError(formatString, exception.Message);
    }
}