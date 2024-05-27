using Core.Domain.Abstract;

namespace Domain.DTOs.Communication;

public class CommentCreateDto : IDto
{
    public Guid? DutyId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ReplyToId { get; set; }
    public string? Text { get; set; }
}