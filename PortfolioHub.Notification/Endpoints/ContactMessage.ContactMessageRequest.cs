namespace PortfolioHub.Notification.Endpoints;

internal sealed record ContactMessageRequest(
    string Name,
    string Email,
    string Subject,
    string Message,
    string Token 
);
