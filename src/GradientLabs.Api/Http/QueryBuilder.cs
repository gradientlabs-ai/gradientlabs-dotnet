using System.Web;

namespace GradientLabs.Api.Http;

internal static class QueryBuilder
{
    internal static string Build(Dictionary<string, string?> parameters)
    {
        var pairs = parameters
            .Where(kv => kv.Value is not null)
            .Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}");
        var qs = string.Join("&", pairs);
        return qs.Length > 0 ? "?" + qs : string.Empty;
    }
}
