using System.Text.Json;
using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Web.Infra;

internal record RecaptchaResponse(
    bool? Success,
    double? Score,
    string? Action,
    DateTime? Challenge_TS,
    string? Hostname,
    List<string>? ErrorCodes
);

internal sealed class GoogleRecaptchaValidator(
    HttpClient httpClient,
    IConfiguration config

    ) : ICaptchaValidator
{
    public async Task<bool> IsValidAsync(string token, string? action = null,
        CancellationToken cancellationToken = default, string? remoteIp = null)
    {
        Guard.Against.NullOrEmpty(token);

        var secretKey = config["GoogleRecaptcha:SecretKey"]
            ?? throw new InvalidOperationException("Google Recaptcha Secret Key is not configured.");
        var verificationUrl = config["GoogleRecaptcha:SiteVerifyUrl"]
            ?? throw new InvalidOperationException("Google Recaptcha Site Verify URL is not configured.");

        var values = new Dictionary<string, string>
        {
            { "secret", secretKey },
            { "response", token }
        };
        if (!string.IsNullOrWhiteSpace(remoteIp))
            values.Add("remoteip", remoteIp);

        using var content = new FormUrlEncodedContent(values);
        using var response = await httpClient.PostAsync(verificationUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<RecaptchaResponse>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result is null || result.Success != true)
            return false;

        if (result.Score is not null && result.Score < 0.5)
            return false;

        if (!string.IsNullOrWhiteSpace(action) && !string.Equals(result.Action, action, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}
