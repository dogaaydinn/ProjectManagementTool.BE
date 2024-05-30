using Domain.DTOs.ProjectManagement;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.ProjectManagement;

public class ProjectCreateValidator : AbstractValidator<ProjectCreateDto>
{
    public ProjectCreateValidator()
    {
        RuleFor(project => project.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 100).WithMessage("Name should be between 3 and 100 characters in length");

        RuleFor(project => project.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(3, 500).WithMessage("Description should be between 3 and 500 characters in length");

        RuleFor(project => project.StartDate)
            .NotEmpty().WithMessage("StartDate is required.")
            .GreaterThan(DateTime.Now).WithMessage("StartDate should be in the future");
    }
}