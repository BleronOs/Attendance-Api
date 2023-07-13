using System.Text.RegularExpressions;

namespace AttendanceTracker.Api.Utils;

/// <summary>
/// If the controller name is TestingWay it will become testing-way
/// </summary>
public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
    {
        return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}