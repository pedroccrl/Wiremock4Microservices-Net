using System.Reflection;

namespace WiremockMicroservices;

internal static class WiremockContext
{
    internal static Assembly[]? AssembliesEndpoints { get; set; }
}