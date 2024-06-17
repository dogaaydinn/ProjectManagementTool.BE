using Domain.DTOs.Auth;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.Authentication;

public class VerifyMfaCodeValidator : AbstractValidator<VerifyMfaCodeDto>
{
    public VerifyMfaCodeValidator()
    {
        RuleFor(mfa => mfa.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Length(6).WithMessage("Code should be 6 digits long.")
            .Matches(@"^\d{6}$").WithMessage("Code should only contain digits.");

        RuleFor(mfa => mfa.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");
    }
}