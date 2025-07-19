using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Infrastructure.EFRepository;

internal class EFProjectRepo(
    ProjectsDbContext dbContext
    ) : IProjectsRepo
{
    public async Task<Result> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(project);
        Guard.Against.Default(project.Id);

        try
        {
            var result = await dbContext.Projects.AddAsync(project, cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            // Optionally log the exception here
            return Result.Error($"Failed to add project: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        var project = await dbContext.Projects
            .Include(p => p.Images)
            .Include(p => p.Skills)
            .Include(p => p.Links)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (project is null)
            return Result.NotFound("Project is not found");

        dbContext.Projects.Remove(project);
        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        var result = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (result is not null)
            return Result.Success(true);

        return Result.Success(false);
    }

    public async Task<Result<IReadOnlyList<Project>>> GetAllAsync(int pageNumber, int pageSize, Guid? categoryId = null,
        string? search = null, bool isFeatured = false, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            return Result.Invalid(new ValidationError("Page number must be greater than zero."));
        if (pageSize <= 0)
            return Result.Invalid(new ValidationError("Page size must be greater than zero."));

        var queryable = dbContext.Projects
           .Include(p => p.Images)
           .Include(p => p.Skills)
           .Include(p => p.Links)!
               .ThenInclude(l => l.LinkProvider)
           .Include(p => p.Category)
           .OrderByDescending(p => p.CreatedDate)
           .AsQueryable();

        if (categoryId.HasValue)
            queryable = queryable.Where(p => p.Category != null && p.Category.Id == categoryId.Value);

        if (!string.IsNullOrEmpty(search))
            queryable = queryable.Where(p => p.Title.Contains(search) || p.Description.Contains(search));

        if (isFeatured)
            queryable = queryable.Where(p => p.IsFeatured == isFeatured);

        var projects = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<Project>>(projects);
    }

    public async Task<Result<Project>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        var project = await dbContext.Projects
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Skills)
            .Include(p => p.Links)!
            .ThenInclude(l => l.LinkProvider)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (project is not null)
            return Result.Success(project);

        return Result<Project>.NotFound();
    }



    public async Task<Result> UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(project);
        Guard.Against.Default(project.Id);

        var existingProject = await dbContext.Projects
            .Include(p => p.Images)
            .Include(p => p.Skills)
            .Include(p => p.Links)
            .FirstOrDefaultAsync(p => p.Id == project.Id, cancellationToken);

        if (existingProject is null)
            return Result.NotFound();

        // Update scalar properties
        existingProject.SetTitle(project.Title);
        existingProject.SetDescription(project.Description);
        existingProject.SetLongDescription(project.LongDescription);
        existingProject.SetVideoUrl(project.VideoId);
        existingProject.SetCoverImageUrl(project.CoverImageUrl);
        existingProject.SetCreatedDate(project.CreatedDate);

        // Update Images
        dbContext.Galleries.RemoveRange(existingProject.Images);
        if (project.Images != null && project.Images.Count > 0)
        {
            foreach (var image in project.Images)
            {
                dbContext.Galleries.Add(new Gallery(image.Id, image.ImageUrl, image.Order));
            }
        }

        // Update Skills
        existingProject.Skills.Clear();
        if (project.Skills != null && project.Skills.Count > 0)
        {
            foreach (var skill in project.Skills)
            {
                var trackedSkill = await dbContext.TechnicalSkills.FindAsync(new object[] { skill.Id }, cancellationToken);
                if (trackedSkill != null)
                    existingProject.Skills.Add(trackedSkill);
            }
        }

        // Update Links
        dbContext.Links.RemoveRange(existingProject.Links);
        if (project.Links != null && project.Links.Count > 0)
        {
            foreach (var link in project.Links)
            {
                dbContext.Links.Add(new Links(link.Id, link.Url));
            }
        }

        return Result.Success();
    }
    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result > 0)
            return Result.Success();
        else
            return Result.Error("No changes were made to the database.");
    }

    public async Task<Result<int>> GetTotalCount(CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await dbContext.Projects.CountAsync(cancellationToken);
            return Result.Success(count);
        }
        catch (Exception ex)
        {
            // Optionally log the exception here
            return Result<int>.Error($"Failed to get total project count: {ex.Message}");
        }
    }
}
