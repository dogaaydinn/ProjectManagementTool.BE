using System.ComponentModel.DataAnnotations;
using Core.Constants;
using Core.Constants.Duty;
using Core.Domain.Abstract;
using Domain.Entities.Communication;
using Domain.Entities.DutyManagement.UserManagement;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.DutyManagement;

public class Duty : EntityBase
{
    [StringLength(127)] public string Title { get; set; } = null!;
    [StringLength(127)] public string Description { get; set; } = null!;
    public DutyType DutyType { get; set; } // Could be an enum for different task types
    public DateTime DueDate { get; set; } // The task has a due date
    public Priority Priority { get; set; } // The task has a priority
    public Project? Project { get; set; } // The task belongs to a project
    public Guid ProjectId { get; set; } // Foreign key referencing Project.Id
    public User Reporter { get; set; } // The task is reported by a user
    public Guid ReporterId { get; set; } // The task is reported by a user
    public DutyStatus Status { get; set; } = DutyStatus.ToDo;
    public Guid? ParentDutyId { get; set; } // The task has a parent task
    public Duty ParentDuty { get; set; } // The task has a parent task
    public bool IsDeleted { get; set; } // The task can be deleted
    public virtual ICollection<Comment>? Comments { get; set; } // The task has several comments
    public virtual ICollection<string> Labels { get; set; } // The task has several labels
    public virtual ICollection<User>? AssignedUsers { get; set; } // The task has several users
    public virtual ICollection<Duty> SubDuties { get; set; }
}