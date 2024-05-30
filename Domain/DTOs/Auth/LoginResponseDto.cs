using Core.Domain.Abstract;
using Core.Security.SessionManagement;
using Domain.DTOs.DutyManagement.UserManagement;

namespace Domain.DTOs.Auth;

public class LoginResponseDto: IDto
{
    public UserGetDto User { get; set; } = null!;
    public Token? Token { get; set; } = null!;
}