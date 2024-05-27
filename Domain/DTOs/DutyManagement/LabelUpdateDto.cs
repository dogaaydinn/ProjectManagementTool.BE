using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement;

public class LabelUpdateDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public string? Description { get; set; }
}