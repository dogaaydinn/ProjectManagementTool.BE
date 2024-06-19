using Core.Constants;
using Core.Constants.Duty;
using Core.Constants.Project;
using Core.Domain.Abstract;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.DTOs.ProjectManagement;

public class ProjectGetDto : IDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public ProjectStatus Status { get; set; }
    public Priority Priority { get; set; }
    public Guid ManagerId { get; set; }
    public User Manager { get; set; }
    public Guid Id { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid CreatedUserId { get; set; }
    public Guid? DeletedUserId { get; set; }
    public Guid? UpdatedUserId { get; set; }
}