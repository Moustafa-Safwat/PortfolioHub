using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed record BlogPostBlockDto
(
    string Type,
    string Text,
    int Order,
    string? Url,
    string? FileName,
    string? MimeType,
    string? CodeTitle,
    string? CodeLanguage,
    string? TextAlign
);

internal sealed record BlogPostBlockIdDto
(
    Guid Id,
    string Type,
    string Text,
    int Order,
    string? Url,
    string? FileName,
    string? MimeType,
    string? CodeTitle,
    string? CodeLanguage,
    string? TextAlign
);