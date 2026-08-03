using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users;

public sealed record GetUsersByIdQuery
(
    IEnumerable<Guid> UserIds
) : IQuery<IEnumerable<GetUserDto>>;

public sealed record GetUserDto
(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email
);
