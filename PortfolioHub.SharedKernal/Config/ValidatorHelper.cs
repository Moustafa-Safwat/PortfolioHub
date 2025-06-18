using System.Text.RegularExpressions;

namespace PortfolioHub.SharedKernal.Config;

public static class ValidatorHelper
{
    private static readonly Regex UrlRegex = new Regex(
        @"^(https?:\/\/)" +                     // Scheme
        @"(([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,})" +  // Domain
        @"(\/[\w\-._~:/?#\[\]@!$&'()*+,;=.]*)?$", // Path and query
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool BeAValidUrl(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
               && UrlRegex.IsMatch(url);
    }
}
