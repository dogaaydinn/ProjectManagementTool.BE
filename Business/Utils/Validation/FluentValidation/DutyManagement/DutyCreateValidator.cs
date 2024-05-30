using Domain.DTOs.DutyManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.DutyManagement;

    public class DutyCreateValidator : AbstractValidator<DutyCreateDto>
    {
        public DutyCreateValidator()
        {
            RuleFor(duty => duty.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title should be between 3 and 100 characters in length");

            RuleFor(duty => duty.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(3, 500).WithMessage("Description should be between 3 and 500 characters in length");

            RuleFor(duty => duty.DueDate)
                .NotEmpty().WithMessage("DueDate is required.")
                .GreaterThan(DateTime.Now).WithMessage("DueDate should be in the future");
        }
    }
