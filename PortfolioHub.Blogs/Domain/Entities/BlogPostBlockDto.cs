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
