using Core.Constants.Duty;
using Core.Domain.Abstract;

namespace Domain.Entities.DutyManagement;

public class Label : EntityBase
{
    public string Name { get; set; }
    public string Color { get; set; } = LabelColors.White;
    public virtual ICollection<Duty> Duties { get; set; }
    public string Description { get; set; } 
}