using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed class AddProfessionalSkillCommandHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> repository
    ) : IRequestHandler<AddProfessionalSkillCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(AddProfessionalSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = new Domain.Entities.ProfessionalSkill(Guid.NewGuid(), request.name);

        await repository.AddAsync(skill, cancellationToken);
        var effectedRowsResult = await repository.SaveChangesAsync(cancellationToken);

        if (!effectedRowsResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(effectedRowsResult.Errors));

        return Result<Guid>.Success(skill.Id);
    }
}