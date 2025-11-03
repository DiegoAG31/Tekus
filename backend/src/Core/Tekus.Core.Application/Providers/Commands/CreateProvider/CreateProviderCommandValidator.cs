using FluentValidation;

namespace Tekus.Core.Application.Providers.Commands.CreateProvider;

/// <summary>
/// Validator for CreateProviderCommand
/// </summary>
public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
{
    public CreateProviderCommandValidator()
    {
        RuleFor(x => x.Nit)
            .NotEmpty().WithMessage("NIT is required")
            .MaximumLength(20).WithMessage("NIT cannot exceed 20 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");
    }
}