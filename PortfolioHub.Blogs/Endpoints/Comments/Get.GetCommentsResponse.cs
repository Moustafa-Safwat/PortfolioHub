namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record GetCommentsResponse
(
    Guid Id,
    Guid UserId,
    string UserFirstName,
    string UserLastName,
    string Comment,
    DateTime CreatedAtUtc,
    bool IsEdited,
    IReadOnlyCollection<GetCommentsResponse> Replies
);
