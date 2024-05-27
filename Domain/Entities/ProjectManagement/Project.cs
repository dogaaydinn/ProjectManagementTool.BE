using System.ComponentModel.DataAnnotations;
using Core.Constants.Duty;
using Core.Constants.Project;
using Core.Domain.Abstract;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.Entities.ProjectManagement;

public class Project : EntityBase
{
    [StringLength(127)] public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public ProjectStatus Status { get; set; } // Could be an enum for different statuses
    public Priority Priority { get; set; } // Consider using a constant enum for priorities
    public Guid ManagerId { get; set; } // Foreign key referencing User.Id
    public User Manager { get; set; } // The project's manager
    public virtual ICollection<Team>? TeamsThatCanAccess { get; set; } // The teams that can access the project
    public virtual ICollection<Duty>? Duties { get; set; } // The project's tasks
}