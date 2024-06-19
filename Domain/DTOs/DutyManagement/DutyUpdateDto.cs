using Core.Constants;
using Core.Constants.Duty;
using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement;

public class DutyUpdateDto : IDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public DutyType? Type { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority? Priority { get; set; }
    public DutyStatus? Status { get; set; }
}