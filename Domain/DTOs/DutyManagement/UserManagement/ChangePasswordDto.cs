using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement.UserManagement;

public class ChangePasswordDto : IDto
{
    public Guid Id { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}