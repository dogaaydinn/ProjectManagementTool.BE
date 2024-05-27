using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class LoginDto: IDto
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; } = null!;
}