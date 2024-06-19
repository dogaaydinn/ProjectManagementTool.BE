using Core.Domain.Abstract;

namespace Domain.DTOs.Auth;

public class RegisterDto : IDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? ProfilePicture { get; set; } = null!;
}