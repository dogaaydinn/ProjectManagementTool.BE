using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement;

public class LabelCreateDto : IDto
{
    public string Name { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
}