using Domain.DTOs.Auth;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.Authentication;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid");
    }
}