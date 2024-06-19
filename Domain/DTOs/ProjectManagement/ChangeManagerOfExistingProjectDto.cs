using Core.Domain.Abstract;

namespace Domain.DTOs.ProjectManagement;

public class ChangeManagerOfExistingProjectDto : IDto
{
    public Guid ProjectId { get; set; }
    public Guid NewManagerId { get; set; }
}