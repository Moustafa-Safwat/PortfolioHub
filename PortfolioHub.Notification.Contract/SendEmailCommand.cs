using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Notification.Usecases;

public sealed record SendEmailCommand(
    string From,
    string To,
    string Subject,
    string Body) : IRequest<Result<Guid>>;
