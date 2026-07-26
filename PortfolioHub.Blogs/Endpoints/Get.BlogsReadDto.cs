namespace PortfolioHub.Blogs.Endpoints;

internal sealed record BlogsReadDto(
    Guid Id,
    string Title,
    string Description,
    DateTime PublishedAt,
    int ReadTime,
    int LikesNo,
    int commentsNo,
    IReadOnlyCollection<string> Tags);
