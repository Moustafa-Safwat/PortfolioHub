using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.ProfessionalSkills;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;
internal sealed class Delete(
    ISender sender
    ) : Endpoint<DeleteProfessionalSkillRequest, Result>
{
    public override void Configure()
    {
        Delete("/professional-skills/{Id}");
        Roles("admin");
    }
    public override async Task HandleAsync(DeleteProfessionalSkillRequest req, CancellationToken ct)
    {
        var deleteCommand = new DeleteProfessionalSkillCommand(Guid.Parse(req.Id));
        var result = await sender.Send(deleteCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        var successObj = Result.SuccessWithMessage("The professional skill entry was deleted successfully.");
        await SendOkAsync(successObj, ct);
    }
}
