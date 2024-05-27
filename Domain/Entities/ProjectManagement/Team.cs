using System.ComponentModel.DataAnnotations;
using Core.Domain.Abstract;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.Entities.ProjectManagement;

public class Team : EntityBase
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid(); // Primary key
    [StringLength(127)] public string Name { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; } 
    public bool IsDeleted { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public Guid ManagerId { get; set; } // Foreign key referencing User.Id
    public User Manager { get; set; } // The team's manager
    public virtual ICollection<Project>? AccessibleProjects { get; set; } // The team's projects
    public virtual ICollection<User> Members { get; set; } // The team's users
}