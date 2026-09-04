using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PortfolioHub.Web.Infra;

internal static class CustomizeSerilogLoggingRequests
{
    private static string GetUserInfo(this HttpContext httpContext, string claim)
    {
        bool hasAuthHeader = httpContext.Request.Headers.ContainsKey("Authorization");
        if (!hasAuthHeader) return "Anonymous";

        string? authHeaderValue = httpContext.Request.Headers["Authorization"].FirstOrDefault();

        Func<bool> isNotValidAuthHeader = () =>
        {
            return string.IsNullOrEmpty(authHeaderValue) ||
                   !authHeaderValue.StartsWith("Bearer ");
        };

        if (isNotValidAuthHeader()) return "Anonymous";

        var token = authHeaderValue!.Substring("Bearer ".Length).Trim();

        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == claim)?.Value ?? "Anonymous";
        }
        catch
        {
            return "Anonymous";
        }
    }

    public static void CustomizeLoggingRequests(this IDiagnosticContext diagnosticContext,
        HttpContext httpContext)
    {
        // User Data
        diagnosticContext.Set("UserId",
            httpContext.GetUserInfo(ClaimTypes.NameIdentifier));
        diagnosticContext.Set("UserEmail",
            httpContext.GetUserInfo(ClaimTypes.Email));
        diagnosticContext.Set("UserName",
            httpContext.GetUserInfo(ClaimTypes.Name));
        diagnosticContext.Set("UserFullName",
            $"{httpContext.GetUserInfo("FirstName")} {httpContext.GetUserInfo("LastName")}");
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
