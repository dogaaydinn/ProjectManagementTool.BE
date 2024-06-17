using Core.Constants.Duty;
using Core.Domain.Abstract;
using Domain.DTOs.DutyManagement.UserManagement;

namespace Domain.DTOs.DutyManagement;

public class DutyGetDto : IDto
{
    public Guid Id { get; set; } = default!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DutyType Type { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Guid ProjectId { get; set; }
    public Guid ReporterId { get; set; }
    public DutyStatus Status { get; set; }
    public Guid AssigneeId { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<UserGetDto?> AssignedUsers { get; set; }
    public Guid? ParentDutyId { get; set; }
}