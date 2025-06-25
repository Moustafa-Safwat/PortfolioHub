namespace PortfolioHub.Achievements.Endpoints.Certificate;

internal sealed record AddCertificateRequest(
    string Name,
    string Issuer,
    DateTime Date
    );
