using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Blogs.Infrastructure.DbSeed;

internal sealed class DbBlogTagSeeder
(
    IEntityRepo<BlogPostTag> tagRepo
) : IDbSeeder
{
    private readonly IEnumerable<string> _blogTags = ["BIM", "IFC", "Revit API", "CAD API", "C#", "Backend", "Architecture", "Docker", "AI", "Structure Analysis", "AR", "WPF", "Nginx"];

    public async Task SeedAsync()
    {
        var getAllTagsResult = await tagRepo.GetAllAsync(1, 100);
        if (!getAllTagsResult.IsSuccess)
            throw new InvalidOperationException();

        var getAllTags = getAllTagsResult.Value;
        var tagsToAdd = _blogTags.Except(getAllTags.Select(tag => tag.Name));
        if (tagsToAdd.Any())
        {
            tagsToAdd.ToList()
                .ForEach(async (tag) =>
                {
                    var blogPostTag = new BlogPostTag(tag);
                    await tagRepo.AddAsync(blogPostTag);
                });
            await tagRepo.SaveChangesAsync();
        }
    }
}
