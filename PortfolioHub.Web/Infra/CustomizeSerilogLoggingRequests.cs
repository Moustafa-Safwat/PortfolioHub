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
        diagnosticContext.Set("User_Real_IP",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "X-Real-IP").Value.ToString() ?? "NA");
        diagnosticContext.Set("X-Device-Type",
            httpContext.Request
            .Headers.FirstOrDefault(h => h.Key == "x-device-type").Value.ToString() ?? "NA");
    }
}
