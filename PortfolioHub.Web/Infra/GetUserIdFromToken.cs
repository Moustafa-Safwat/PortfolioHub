using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortfolioHub.Web.Infra;


internal sealed class GetUserIdFromToken : IGetUserIdFromToken
{
    private const string BearerPrefix = "Bearer ";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenValidationParameters _validationParameters;

    public GetUserIdFromToken(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;

        var jwtSecret = configuration["Auth:JwtSecret"];

        if (string.IsNullOrWhiteSpace(jwtSecret))
            throw new InvalidOperationException("The JWT secret is not configured in Auth:JwtSecret.");

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false,
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    }

    public Guid GetOptionalUserId()
    {
        var token = GetBearerToken();

        if (string.IsNullOrWhiteSpace(token))
            return Guid.Empty;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                _validationParameters,
                out var validatedToken);

            if (validatedToken is not JwtSecurityToken)
                return Guid.Empty;

            var userIdClaim =
                principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? principal.FindFirstValue("userId");

            return Guid.TryParse(userIdClaim, out var userId) &&
                   userId != Guid.Empty ? userId : Guid.Empty;
        }
        catch (SecurityTokenException)
        {
            return Guid.Empty;
        }
        catch (ArgumentException)
        {
            return Guid.Empty;
        }
    }

    private string? GetBearerToken()
    {
        var authorizationHeader = _httpContextAccessor
            .HttpContext?
            .Request
            .Headers[HeaderNames.Authorization]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authorizationHeader))
            return null;

        if (!authorizationHeader.StartsWith(
                BearerPrefix,
                StringComparison.OrdinalIgnoreCase))
            return null;

        var token = authorizationHeader[BearerPrefix.Length..].Trim();

        return string.IsNullOrWhiteSpace(token) ? null : token;
    }
}