using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement.UserManagement;

public class ResetPasswordDto: IDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string CurrentPassword { get; set; } = null!;
    public string LoginVerificationCode { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}