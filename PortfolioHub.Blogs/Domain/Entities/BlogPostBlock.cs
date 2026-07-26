using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostBlock : DeletionEntity
{
    public Guid BlogPostId { get; private set; }
    public string Text { get; private set; } = null!;
    public string BlockType { get; private set; } = null!;
    public string? Url { get; private set; }
    public string? FileName { get; private set; }
    public string? MimeType { get; private set; }
    public string? CodeTitle { get; private set; }
    public string? CodeLanguage { get; private set; }
    public string? TextAlign { get; private set; }
    public int Order { get; private set; }
    // Navigation property
    public BlogPost BlogPost { get; private set; } = null!;
    // EF Constructor
    private BlogPostBlock() { }
    public BlogPostBlock(Guid blogPostId, Guid userId, string text, string blockType, int order)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        Text = Guard.Against.NullOrWhiteSpace(text);
        BlockType = Guard.Against.NullOrWhiteSpace(blockType);
        Order = Guard.Against.Negative(order);
        MarkAsCreated(userId);
    }
    // Methods
    public void SetUrl(string url)
    {
        Url = Guard.Against.NullOrWhiteSpace(url);
    }
    public void SetMimeType(string mimeType)
    {
        MimeType = Guard.Against.NullOrWhiteSpace(mimeType);
    }
    public void SetFileName(string fileName)
    {
        FileName = Guard.Against.NullOrWhiteSpace(fileName);
    }
    public void SetCodeTitle(string codeTitle)
    {
        CodeTitle = Guard.Against.NullOrWhiteSpace(codeTitle);
    }
    public void SetCodeLanguage(string codeLanguage)
    {
        CodeLanguage = Guard.Against.NullOrWhiteSpace(codeLanguage);
    }
    public void SetTextAlign(string textAlign)
    {
        TextAlign = Guard.Against.NullOrWhiteSpace(textAlign);
    }
}