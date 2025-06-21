using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Notification.Usecases;

internal sealed record SendEmailCommand(
    string From,
    string To,
    string Subject,
    string Body) : IRequest<Result<Guid>>;
