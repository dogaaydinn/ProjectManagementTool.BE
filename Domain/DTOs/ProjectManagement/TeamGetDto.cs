using System.ComponentModel.DataAnnotations;
using Core.Constants.Duty;
using Core.Constants.Project;
using Core.Domain.Abstract;

namespace Domain.DTOs.ProjectManagement;

public class TeamGetDto : IDto
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public Guid ManagerId { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public DutyType DutyType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsDeleted { get; set; }
}