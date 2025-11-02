using FluentValidation;

namespace Tekus.Core.Application.Services.Commands.AssignCountriesToService;

public class AssignCountriesToServiceCommandValidator : AbstractValidator<AssignCountriesToServiceCommand>
{
    public AssignCountriesToServiceCommandValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("Service ID is required");

        RuleFor(x => x.CountryCodes)
            .NotEmpty().WithMessage("At least one country code is required");

        RuleForEach(x => x.CountryCodes)
            .NotEmpty().WithMessage("Country code cannot be empty")
            .Length(3).WithMessage("Country code must be 3 characters");
    }
}
