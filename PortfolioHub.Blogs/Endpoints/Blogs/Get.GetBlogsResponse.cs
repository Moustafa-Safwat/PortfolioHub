namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record GetBlogsResponse(
    IReadOnlyList<BlogsReadDto> Blogs,
    int TotalCount,
    int PageNumber,
    int PageSize);
