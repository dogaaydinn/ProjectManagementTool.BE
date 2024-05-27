using Core.Domain.Abstract;
using Domain.Entities.DutyManagement;

namespace Domain.Entities.Association;

public class DutyLabels : EntityBase
{
    public Guid DutyId { get; set; }
    public virtual Duty Duty { get; set; } = null!;
    public Guid LabelId { get; set; }
    public virtual Label Label { get; set; } = null!;
}