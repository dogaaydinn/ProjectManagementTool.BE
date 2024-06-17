using Core.Domain.Abstract;

namespace Domain.DTOs.ProjectManagement;

public class AssignUserToProjectDto : IDto
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}