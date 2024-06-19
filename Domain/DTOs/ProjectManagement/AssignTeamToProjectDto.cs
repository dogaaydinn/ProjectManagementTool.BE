using Core.Domain.Abstract;

namespace Domain.DTOs.ProjectManagement;

public class AssignTeamToProjectDto : IDto
{
    public Guid ProjectId { get; set; }
    public Guid TeamId { get; set; }
}