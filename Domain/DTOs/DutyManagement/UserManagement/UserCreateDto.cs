using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement.UserManagement;

public class UserCreateDto: IDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string PhoneNumber { get; set; }
    public bool UseMultiFactorAuthentication { get; set; }
}