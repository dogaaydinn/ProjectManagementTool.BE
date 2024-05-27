using System.ComponentModel.DataAnnotations;
using Core.Domain.Abstract;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.Entities.Communication;

public class Comment : EntityBase
{
    public Guid? DutyId { get; set; } // Foreign key referencing Task.Id
    public virtual Duty? Duty { get; set; } // The comment belongs to a task
    public Guid AuthorId { get; set; } // Foreign key referencing User.Id
    public User Author { get; set; } // Navigation property
    public Guid? ReplyToId { get; set; } // Foreign key for nested comments (optional)
    public Comment? ReplyTo { get; set; } // Navigation property for nested comments (optional)
    [StringLength(127)] public string? Text { get; set; }

    public ICollection<Comment>
        SubComments { get; set; } // Navigation property for one-to-many relationship with nested comments (optional)
}