using Domain.DTOs.Communication;
using FluentValidation;

namespace Business.Utils.Validation.FluentValidation.Communication;

public class CommentUpdateValidator : AbstractValidator<CommentUpdateDto>
{
    public CommentUpdateValidator()
    {
        RuleFor(comment => comment.Text)
            .NotEmpty().WithMessage("Content is required.")
            .Length(1, 500).WithMessage("Content should be between 1 and 500 characters in length");

        RuleFor(comment => comment.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}