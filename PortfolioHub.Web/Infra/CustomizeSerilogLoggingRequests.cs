using System.Security.Claims;
using Serilog;

namespace PortfolioHub.Web.Infra;

internal static class CustomizeSerilogLoggingRequests
{
    public static void CustomizeLoggingRequests(this IDiagnosticContext diagnosticContext,
        HttpContext httpContext)
    {
        Func<string, string> userInfo = (string claim)
            => httpContext?.User?.FindFirst(claim)?.Value ?? "Anonymous";

        // User Data
        diagnosticContext.Set("UserId", userInfo(ClaimTypes.NameIdentifier));
        diagnosticContext.Set("UserEmail", userInfo(ClaimTypes.Email));
        diagnosticContext.Set("UserName", userInfo(ClaimTypes.Name));
        diagnosticContext.Set("UserFullName", $"{userInfo("FirstName")} {userInfo("LastName")}");
        // Location Data
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
        // Info Data
        diagnosticContext.Set("tag", "backend");
    }
}
