using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class VerifyMfaCodeDto : IDto
{
    public string Email { get; set; }
    public string Code { get; set; }
}