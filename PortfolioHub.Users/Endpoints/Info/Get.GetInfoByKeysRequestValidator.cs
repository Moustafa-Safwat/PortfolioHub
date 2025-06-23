using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class GetInfoByKeysRequestValidator
    : Validator<GetInfoByKeysRequest>
{
    public GetInfoByKeysRequestValidator()
    {
        RuleFor(x => x.Keys).NotEmpty().WithMessage("Key cannot be empty.");
        RuleForEach(x => x.Keys).NotEmpty().WithMessage("Each key must not be empty.");
    }
}


