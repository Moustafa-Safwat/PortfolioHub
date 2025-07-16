using System.Security.Claims;
using Serilog;

namespace PortfolioHub.Web.Infra;

internal static class CustomizeSerilogLoggingRequests
{
    public static void CustomizeLoggingRequests(this IDiagnosticContext diagnosticContext,
        HttpContext httpContext)
    {
        diagnosticContext.Set("UserId",
         httpContext.User
         .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous");
        diagnosticContext.Set("X-Device-Type",
            httpContext.Request.Headers.FirstOrDefault(h => 
            string.Equals(h.Key, "x-device-type", StringComparison.OrdinalIgnoreCase))
            .Value.ToString() ?? "NA");
        diagnosticContext.Set("User_Real_IP",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Real-IP").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-Country-Code",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Country-Code").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-Country-Name",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Country-Name").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-City",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-City").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-Latitude",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Latitude").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-Longitude",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Longitude").Value.ToString() ?? "NA");
        diagnosticContext.Set("tag", "backend");
    }
}
