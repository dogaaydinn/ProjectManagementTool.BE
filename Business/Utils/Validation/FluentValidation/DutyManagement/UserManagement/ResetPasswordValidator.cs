using Domain.DTOs.DutyManagement.UserManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement.UserManagement;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        RuleFor(x => x.NewPassword).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
            .WithMessage(
                "Password should be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character");
    }
}