using Core.Constants.Duty;
using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement;

public class DutyCreateDto : IDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DutyType Type { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Guid ProjectId { get; set; }
    public Guid ReporterId { get; set; }
    public DutyStatus Status { get; set; }
    public Guid? ParentDutyId { get; set; }
}