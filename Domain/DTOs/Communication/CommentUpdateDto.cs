using Core.Domain.Abstract;

namespace Domain.DTOs.Communication;

public class CommentUpdateDto : IDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
}