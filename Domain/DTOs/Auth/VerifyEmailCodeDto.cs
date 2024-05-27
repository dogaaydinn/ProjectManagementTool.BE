using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class VerifyEmailCodeDto : IDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}