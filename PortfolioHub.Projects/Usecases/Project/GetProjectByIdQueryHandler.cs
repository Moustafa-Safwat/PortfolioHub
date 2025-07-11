using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class GetProjectByIdQueryHandler(
    IProjectsRepo projectsRepo
    ) : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var projectRes = await projectsRepo.GetByIdAsync(request.Id, cancellationToken);
        if (!projectRes.IsSuccess)
            return Result.Error(new ErrorList(projectRes.Errors));

        var project = projectRes.Value;
        var projectDto = new ProjectDto(
            project.Title,
            project.Description,
            project.LongDescription,
            project.CreatedDate,
            new Endpoints.Category.CategoryDto(
                project.Category!.Id,
                project.Category.Name
            ),
            project.VideoUrl,
            project.CoverImageUrl,
            project.Images!.OrderBy(i => i.Order).Select(i => i.ImageUrl).ToArray(),
            project.Skills!.Select(s => new TechSkillDto(s.Id, s.Name)).ToArray(),
            project.Links!.Select(l => new LinkDto(
                l.LinkProvider!.Id.ToString(),
                l.Url,
                l.LinkProvider.Name,
                l.LinkProvider.BaseUrl
            )).ToArray()
        );

        return Result.Success(projectDto);
    }
}
