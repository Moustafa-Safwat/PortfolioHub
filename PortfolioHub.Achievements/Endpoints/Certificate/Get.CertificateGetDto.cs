namespace PortfolioHub.Achievements.Endpoints.Certificate;

internal sealed record CertificateGetDto(
    Guid Id,
    string Name,
    string Issuer,
    DateTime Date
);
