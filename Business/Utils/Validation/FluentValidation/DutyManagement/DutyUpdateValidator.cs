using Domain.DTOs.DutyManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement;

public class DutyUpdateValidator : AbstractValidator<DutyUpdateDto>
{
    public DutyUpdateValidator()
    {
        RuleFor(duty => duty.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title should be between 3 and 100 characters in length");

        RuleFor(duty => duty.Description)
            .Length(3, 500).WithMessage("Description should be between 3 and 500 characters in length")
            .Unless(duty => string.IsNullOrEmpty(duty.Description));

        RuleFor(duty => duty.DueDate)
            .NotEmpty().WithMessage("DueDate is required.")
            .GreaterThan(DateTime.Now).WithMessage("DueDate should be in the future");
    }
}