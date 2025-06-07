using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class DeleteProjectCommandHandler(
    IProjectsRepo projectsRepo,
    ILogger logger
) : IRequestHandler<DeleteProjectCommand, Result>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        logger
            .ForContext("ProjectId", request.Id)
            .Information("Attempting to delete project");

        var res = await projectsRepo.DeleteAsync(request.Id, cancellationToken);
        var saveRes = await projectsRepo.SaveChangesAsync(cancellationToken);

        if (!(res.IsSuccess && saveRes.IsSuccess))
        {
            logger
                .ForContext("ProjectId", request.Id)
                .ForContext("Errors", res.Errors, destructureObjects: true)
                .Warning("Failed to delete project");
            return res;
        }

        logger
            .ForContext("ProjectId", request.Id)
            .Information("Successfully deleted project");
        return Result.Success();
    }
}
