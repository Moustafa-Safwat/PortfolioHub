using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed class AddAchevReqValidator : Validator<AddAchevReq>
{
    public AddAchevReqValidator()
    {
        RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required.");
        RuleFor(x => x.Institution).NotEmpty().WithMessage("Institution is required.");
        RuleFor(x => x.FieldOfStudy).NotEmpty().WithMessage("Field of study is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.StartDate).NotNull().WithMessage("Start date is required.")
            .Must(date => date >= new DateTime(1900, 1, 1)).WithMessage("Start date must be after 1900.");
        RuleFor(x => x.EndDate).Must((req, endDate) =>
            endDate == null || endDate >= req.StartDate)
            .WithMessage("End date must be after start date or null if ongoing.");
    }
}
