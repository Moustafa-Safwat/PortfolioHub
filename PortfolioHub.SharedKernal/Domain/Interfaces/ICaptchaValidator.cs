namespace PortfolioHub.SharedKernal.Domain.Interfaces;

public interface ICaptchaValidator
{
    Task<bool> IsValidAsync(string token, string? action = null,
        CancellationToken cancellationToken = default, string? remoteIp = null);
}
