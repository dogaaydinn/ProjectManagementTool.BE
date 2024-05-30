using Core.Domain.Abstract;
using Domain.DTOs.DutyManagement;
using Domain.Entities.Communication;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.DTOs.Communication;

public class CommentGetDto : IDto
{
    public Guid Id { get; set; } = default!;
    public Guid DutyId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ReplyToId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Duty? Duty { get; set; }
    public User? Author { get; set; }
    public Comment? ReplyTo { get; set; }
    public ICollection<DutyGetDto?> Reporter { get; set; }
    public List<CommentGetDto> SubComments { get; set; } = new List<CommentGetDto>();
}