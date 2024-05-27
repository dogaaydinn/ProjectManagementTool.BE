using Core.Domain.Abstract;
using Core.Security.SessionManagement;
using Domain.DTOs.DutyManagement.UserManagement;

namespace Domain.DTOs.Auth;

public class LoginResponseDto: IDto
{
    public UserGetDto User { get; set; }
    public Token? Token { get; set; }
}