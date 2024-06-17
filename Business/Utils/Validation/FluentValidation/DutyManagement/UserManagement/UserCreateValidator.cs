using Domain.DTOs.DutyManagement.UserManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement.UserManagement;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
    {
        #region Username

        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 25).WithMessage("Username should be between 3 and 25 characters in length.");

        #endregion

        #region Email

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email not valid.")
            .Length(1, 250).WithMessage("Email maximum length is 250 characters.")
            .WithMessage("Email already exists.");

        #endregion

        #region Password

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage(
                "Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and one special character");

        #endregion

        #region FirstName

        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("FirstName is required.")
            .NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.FirstName).NotEmpty().OverridePropertyName("firstName").WithMessage("FirstName is required");
        RuleFor(x => x.FirstName).Must(x => char.IsLetter(x.First())).OverridePropertyName("firstName")
            .WithMessage("FirstName must begin with a letter");
        RuleFor(x => x.FirstName).Must(x => char.IsLetter(x.Last())).OverridePropertyName("firstName")
            .WithMessage("FirstName cannot contain special characters at the end");
        RuleFor(x => x.FirstName).Must(x => !x.Contains(' ')).OverridePropertyName("firstName")
            .WithMessage("Spaces are not allowed");
        RuleFor(x => x.FirstName).Must(x => x.Length >= 3 && x.Length <= 25).OverridePropertyName("firstName")
            .WithMessage("FirstName should be between 3 and 25 characters in length");

        #endregion

        #region LastName

        RuleFor(x => x.LastName)
            .NotNull().WithMessage("LastName is required.")
            .NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.LastName).Must(x => char.IsLetter(x.First())).OverridePropertyName("lastName")
            .WithMessage("Last name must begin with a letter");
        RuleFor(x => x.LastName).Must(x => char.IsLetter(x.Last())).OverridePropertyName("lastName")
            .WithMessage("Last name cannot contain special characters at the end");
        RuleFor(x => x.LastName).Must(x => !x.Contains(' ')).OverridePropertyName("lastName")
            .WithMessage("Spaces are not allowed");
        RuleFor(x => x.LastName).Must(x => x.Length >= 3 && x.Length <= 25).OverridePropertyName("lastName")
            .WithMessage("Last name should be between 3 and 25 characters in length");

        #endregion

        #region PhoneNumber

        RuleFor(x => x.PhoneNumber).Matches(@"^\+(?:[0-9]●?){6,14}[0-9]$")
            .WithMessage("Phone number should be a valid phone number");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required");
        RuleFor(x => x.UseMultiFactorAuthentication).NotNull()
            .WithMessage("UseMultiFactorAuthentication must be specified");

        #endregion

        #region ProfilePicture

        RuleFor(x => x.ProfilePicture).Matches(@"^.*\.(jpg|jpeg|png)$")
            .WithMessage("Cover photo should be a valid image file. Supported formats are jpg, jpeg, png");

        #endregion
    }
}