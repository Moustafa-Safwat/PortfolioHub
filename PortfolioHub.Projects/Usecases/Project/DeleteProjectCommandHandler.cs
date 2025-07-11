using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class DeleteProjectCommandHandler(
    IProjectsRepo projectsRepo
) : IRequestHandler<DeleteProjectCommand, Result>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var res = await projectsRepo.DeleteAsync(request.Id, cancellationToken);
        var saveRes = await projectsRepo.SaveChangesAsync(cancellationToken);

        if (!(res.IsSuccess && saveRes.IsSuccess))
            return res;

        return Result.Success();
    }
}
