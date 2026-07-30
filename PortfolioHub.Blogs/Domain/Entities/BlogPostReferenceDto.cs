namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed record BlogPostReferenceDto
(
    string Label,
    string Url
);

internal sealed record BlogPostReferenceIdDto
(
    Guid Id,
    string Label,
    string Url
);