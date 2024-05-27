using Domain.DTOs.Auth;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.Authentication;

public class VerifyCodeValidator : AbstractValidator<VerifyEmailCodeDto>
{
    public VerifyCodeValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Code).NotEmpty().WithMessage("MFA code is required");
        RuleFor(x => x.Code).Matches(@"^\d{6}$").WithMessage("MFA code is invalid");
    }
}
