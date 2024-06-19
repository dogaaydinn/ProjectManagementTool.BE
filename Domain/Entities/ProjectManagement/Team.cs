using System.ComponentModel.DataAnnotations;
using Core.Domain.Abstract;
using Domain.Entities.Association;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.Entities.ProjectManagement;

public class Team : EntityBase
{
    [StringLength(127)] public string Name { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public bool IsDeleted { get; set; }
    public Guid ManagerId { get; set; } // Foreign key referencing User.Id
    public User Manager { get; set; } // The team's manager
    //TODO: adar gençayı izledikten sonra members bak, accesible projects için revise gerekli mi kontrol et
    public virtual ICollection<UserTeam> Users { get; set; } = new List<UserTeam>();
    public virtual ICollection<Project>? AccessibleProjects { get; set; } // The team's projects
}