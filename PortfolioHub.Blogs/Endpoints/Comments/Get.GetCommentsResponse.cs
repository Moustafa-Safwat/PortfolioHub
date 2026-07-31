namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record GetCommentsResponse
(
    Guid Id,
    string UserFirstName,
    string UserLastName,
    string Comment,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<GetCommentsResponse> Replies
);
