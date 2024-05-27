using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class ResetPasswordDto : IDto
{
    public string Email { get; set; } = null!;
    public string LoginVerificationCode { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}