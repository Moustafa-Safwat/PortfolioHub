using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record BlogAuthorDto
(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    BlogPostAuthorRole Role
);
