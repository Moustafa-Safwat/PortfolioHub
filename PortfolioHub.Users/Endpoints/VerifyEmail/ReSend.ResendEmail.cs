namespace PortfolioHub.Users.Endpoints.VerifyEmail;

internal sealed record ResendEmail
(
    string UserId,
    string Token
);
