using Core.Services;
using Core.Services.Result;
using Domain.DTOs.Auth;

namespace Business.Services.Auth.Abstract;

public interface IAuthService: IService
{ 
    Task<ServiceObjectResult<LoginResponseDto?>> LoginAsync(LoginDto loginDto);
    Task<ServiceObjectResult<LoginResponseDto?>> RegisterAsync(RegisterDto registerDto); 
    Task<ServiceObjectResult<LoginResponseDto?>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<ServiceObjectResult<LoginResponseDto?>> VerifyEmailCodeAsync(VerifyEmailCodeDto verifyEmailCodeDto);
    Task<ServiceObjectResult<LoginResponseDto?>> VerifyMfaCodeAsync(VerifyMfaCodeDto verifyMfaCodeDto);
    Task<ServiceObjectResult<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<ServiceObjectResult<bool>> LogoutAsync(LogoutDto logoutDto);
}