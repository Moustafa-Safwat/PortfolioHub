using FluentValidation;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class InfoDtoValidator : AbstractValidator<InfoAddDto>
{
    public InfoDtoValidator()
    {
        RuleFor(i => i.InfoKey).NotEmpty().WithMessage("InfoKey cannot be empty.");
        RuleFor(i => i.InfoValue).NotEmpty().WithMessage("InfoValue cannot be empty.");
    }
}
