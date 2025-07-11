using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class AddTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo
    ) : IRequestHandler<AddTechnicalSkillCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        var techSkill = new TechanicalSkills(Guid.NewGuid(), request.Name);

        var addTechSkillsResult = await techSkillRepo.AddAsync(techSkill, cancellationToken);
        if (!addTechSkillsResult.IsSuccess)
            return Result<Guid>.Error("Failed to add technical skill");

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result<Guid>.Error("Failed to add technical skill");

        return Result<Guid>.Success(techSkill.Id);
    }
}