using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class LogoutDto : IDto
{
    public string Token { get; set; }
}