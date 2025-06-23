using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class AddInfoReqValidator : Validator<AddInfoReq>
{
    public AddInfoReqValidator()
    {
        RuleFor(req => req.Infos)
            .NotEmpty().WithMessage("Infos cannot be empty.")
            .ForEach(info =>
            {
                info.SetValidator(new InfoDtoValidator());
            });
    }
}
