using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Certificate;

internal sealed class GetCertificateReqValidator: Validator<GetCertificateReq>
{
    public GetCertificateReqValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1.");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1.");
    }
}
