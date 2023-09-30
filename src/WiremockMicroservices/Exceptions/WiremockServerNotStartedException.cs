using System.Runtime.Serialization;

namespace WiremockMicroservices.Exceptions;

[Serializable]
public class WiremockServerNotStartedException : Exception
{
    public WiremockServerNotStartedException() { }
    
    public WiremockServerNotStartedException(string message) : base(message) { }
    
    public WiremockServerNotStartedException(string message, Exception inner) : base(message, inner) { }
    
    protected WiremockServerNotStartedException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}