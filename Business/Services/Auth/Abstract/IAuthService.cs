using Core.Services;
using Core.Services.Result;
using Domain.DTOs.Auth;

namespace Business.Services.Auth.Abstract;

public interface IAuthService : IService
{
    Task<ServiceObjectResult<LoginResponseDto?>> LoginAsync(LoginDto loginDto);
    Task<ServiceObjectResult<bool>> RegisterAsync(RegisterDto registerDto);
    Task<ServiceObjectResult<LoginResponseDto?>> VerifyMfaCodeAsync(VerifyMfaCodeDto verifyMfaCodeDto);
    Task<ServiceObjectResult<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<ServiceObjectResult<bool>> Logout(Guid userId);
}