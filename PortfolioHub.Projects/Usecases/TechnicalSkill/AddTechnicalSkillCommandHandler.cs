using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class AddTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo,
    ILogger logger
    ) : IRequestHandler<AddTechnicalSkillCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Starting AddTechnicalSkillCommand for skill name: {SkillName}", request.Name);

        var techSkill = new TechanicalSkills(Guid.NewGuid(), request.Name);
        logger.Debug("Created TechanicalSkills entity with Id: {SkillId} and Name: {SkillName}", techSkill.Id, techSkill.Name);

        var addTechSkillsResult = await techSkillRepo.AddAsync(techSkill, cancellationToken);
        if (!addTechSkillsResult.IsSuccess)
        {
            logger.Error("AddAsync failed for technical skill: {SkillName}. Errors: {Errors}", request.Name, addTechSkillsResult.Errors);
            return Result<Guid>.Error("Failed to add technical skill");
        }

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("SaveChangesAsync failed after adding technical skill: {SkillName}. Errors: {Errors}", request.Name, saveResult.Errors);
            return Result<Guid>.Error("Failed to add technical skill");
        }

        logger.Information("Successfully added technical skill with Id: {SkillId} and Name: {SkillName}", techSkill.Id, techSkill.Name);
        return Result<Guid>.Success(techSkill.Id);
    }
}