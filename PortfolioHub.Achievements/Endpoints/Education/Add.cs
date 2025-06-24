using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Education;

namespace PortfolioHub.Achievements.Endpoints.Education;
internal sealed class Add(
    ISender sender
    ) : Endpoint<AddAchevReq, Result<Guid>>
{
    public override void Configure()
    {
        Post("/education");
        Roles("admin");
    }
    public async override Task HandleAsync(AddAchevReq req, CancellationToken ct)
    {
        var addEducationCommand = new AddEducationCommand(
            req.Degree,
            req.Institution,
            req.FieldOfStudy,
            req.Description,
            req.StartDate,
            req.EndDate);

        var addEducationResult = await sender.Send(addEducationCommand, ct);
        if (!addEducationResult.IsSuccess)
        {
            await SendAsync(addEducationResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(addEducationResult, StatusCodes.Status201Created, ct);
    }
}
