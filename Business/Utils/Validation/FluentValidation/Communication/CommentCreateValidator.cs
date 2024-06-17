using Domain.DTOs.Communication;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.Communication;

public class CommentCreateValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateValidator()
    {
        RuleFor(comment => comment.Text)
            .NotEmpty().WithMessage("Content is required.")
            .Length(1, 500).WithMessage("Content should be between 1 and 500 characters in length");
    }
}