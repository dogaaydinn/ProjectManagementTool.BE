using Domain.DTOs.DutyManagement.UserManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement.UserManagement;

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        #region Id

        RuleFor(user => user.Id)
            .NotEmpty().WithMessage("Id is required.");

        #endregion

        #region FirstName

        RuleFor(user => user.FirstName)
            .Length(3, 25)
            .When(user => user.FirstName != null)
            .WithMessage("FirstName should be between 3 and 25 characters in length");

        #endregion

        #region LastName

        RuleFor(user => user.LastName)
            .Length(3, 25)
            .When(user => user.LastName != null)
            .WithMessage("LastName should be between 3 and 25 characters in length");

        #endregion

        #region Email

        RuleFor(user => user.Email)
            .EmailAddress()
            .When(user => user.Email != null)
            .WithMessage("Email is not valid");

        #endregion

        #region Username

        RuleFor(user => user.Username)
            .Length(3, 25)
            .When(user => user.Username != null)
            .WithMessage("Username should be between 3 and 25 characters in length");

        #endregion

        #region Password

        RuleFor(user => user.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .When(user => user.Password != null)
            .WithMessage(
                "Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and one special character");

        #endregion

        #region PhoneNumber

        RuleFor(user => user.PhoneNumber)
            .Matches(@"^(\+[0-9]{1,3})?[0-9]{10,11}$")
            .When(user => user.PhoneNumber != null)
            .WithMessage("PhoneNumber is not valid");

        #endregion
    }
}