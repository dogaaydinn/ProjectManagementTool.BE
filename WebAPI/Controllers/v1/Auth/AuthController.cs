using Business.Services.Auth.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.Auth;

public class AuthController: BaseController
{
    private readonly IAuthService _authService = ServiceTool.GetService<IAuthService>()!;

    #region login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region verify-code
    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyEmailCode([FromBody] VerifyEmailCodeDto verifyEmailCodeDto)
    {
        var result = await _authService.VerifyEmailCodeAsync(verifyEmailCodeDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region verify-mfa
    [HttpPost("verify-mfa")]
    public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaCodeDto verifyMfaCodeDto)
    {
        var result = await _authService.VerifyMfaCodeAsync(verifyMfaCodeDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region forgot-password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region reset-password
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var result = await _authService.ResetPasswordAsync(resetPasswordDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        var result = await _authService.LogoutAsync(logoutDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
}