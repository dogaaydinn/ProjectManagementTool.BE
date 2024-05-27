using Core.Domain.Abstract;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.Association;

public class TeamProject : EntityBase
{
    public Guid TeamId { get; set; } // Foreign key referencing Team.Id
    public virtual Team Team { get; set; } = null!; // Navigation property
    public Guid ProjectId { get; set; } // Foreign key referencing Project.Id
    public virtual Project Project { get; set; } = null!; // Navigation property
}