using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class ForgotPasswordDto : IDto
{
    public string Email { get; set; } = null!;
}