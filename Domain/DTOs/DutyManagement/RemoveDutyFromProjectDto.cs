using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement;

public class RemoveDutyFromProjectDto : IDto
{
    public Guid DutyId { get; set; }
    public Guid ProjectId { get; set; }
}