using Core.Domain.Abstract;
using Domain.Entities.DutyManagement;

namespace Domain.Entities.Association;

public class DutyAccess : EntityBase
{
    public Guid DutyId { get; set; }
    public virtual Duty Duty { get; set; } = null!;
    public TeamProject TeamProject { get; set; } = null!;
    public Guid TeamProjectId { get; set; }
}