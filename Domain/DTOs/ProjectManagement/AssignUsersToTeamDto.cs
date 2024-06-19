using Core.Domain.Abstract;

namespace Domain.DTOs.ProjectManagement;

public class AssignUsersToTeamDto : IDto
{
    public Guid TeamId { get; set; }
    public List<Guid> UserIds { get; set; }
}