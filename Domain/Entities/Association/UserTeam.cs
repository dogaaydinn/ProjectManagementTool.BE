using Core.Domain.Abstract;
using Domain.Entities.DutyManagement.UserManagement;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.Association;

public class UserTeam : EntityBase
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public Guid TeamId { get; set; }
    public virtual Team Team { get; set; } = null!;
}