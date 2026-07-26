namespace PortfolioHub.Blogs.Endpoints;

internal sealed record GetBlogsResponse(
    IReadOnlyList<BlogsReadDto> Blogs,
    int TotalCount,
    int PageNumber,
    int PageSize);
