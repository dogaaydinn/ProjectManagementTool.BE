using Domain.DTOs.DutyManagement.UserManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement.UserManagement;

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        #region FirstName
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .Length(3, 25).WithMessage("FirstName should be between 3 and 25 characters in length");
        #endregion
        
        #region LastName
        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .Length(3, 25).WithMessage("LastName should be between 3 and 25 characters in length");
        #endregion
        
        #region Email
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid");
        #endregion
        
        #region Username
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 25).WithMessage("Username should be between 3 and 25 characters in length");
        #endregion
        
        #region Password
        RuleFor(user => user.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and one special character")
            .Unless(user => string.IsNullOrEmpty(user.Password));
        #endregion
        
        #region PhoneNumber
        RuleFor(user => user.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^(\+[0-9]{1,3})?[0-9]{10,11}$").WithMessage("PhoneNumber is not valid");
        #endregion
        
    }
}