using Core.Domain.Abstract;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.Entities.Association;

// This association entity is used to create a many-to-many relationship between User and Duty entities. 
// A duty can be assigned to multiple users and a user can have multiple duties.
public class UserDuty : EntityBase
{
    public Guid UserId { get; set; } // Foreign key referencing User.Id
    public virtual User User { get; set; } = null!; // Navigation property
    public Guid DutyId { get; set; } // Foreign key referencing Task.Id
    public virtual Duty Duty { get; set; } = null!; // Navigation property
}