using FluentValidation;

namespace AuthenticationService.App.Features.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Surname)
            .NotEmpty().WithMessage("Surname is required")
            .MinimumLength(2).WithMessage("Surname must be at least 2 characters")
            .MaximumLength(64).WithMessage("Surname must be at most 64 characters")
            .Must(BeValidName).WithMessage("Surname must consist only of letters, first letter is uppercase");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(64).WithMessage("Name must be at most 64 characters")
            .Must(BeValidName).WithMessage("Name must consist only of letters, first letter is uppercase");

        RuleFor(c => c.Patronymic)
            .MinimumLength(2).WithMessage("Patronymic must be at least 2 characters")
            .MaximumLength(64).WithMessage("Patronymic must be at most 64 characters")
            .Must(BeValidName).WithMessage("Patronymic must consist only of letters, first letter is uppercase")
            .When(c => !string.IsNullOrEmpty(c.Patronymic));

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required")
            .MinimumLength(5).WithMessage("Email must be at least 5 characters")
            .MaximumLength(255).WithMessage("Email must be at most 255 characters")
            .Matches(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])")
                .WithMessage("Invalid format");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(64).WithMessage("Password must be at most 64 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit");

        RuleFor(c => c.Role)
            .NotEmpty().WithMessage("Role is required")
            .MinimumLength(4).WithMessage("Role must be at least 4 characters")
            .MaximumLength(64).WithMessage("Role must be at most 64 characters")
            .Must(r => r.All(char.IsAsciiLetter)).WithMessage("Role must consist only of latin letters");
    }
    
    private static bool BeValidName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        return char.IsUpper(name[0]) && name.All(char.IsLetter);
    }
}
