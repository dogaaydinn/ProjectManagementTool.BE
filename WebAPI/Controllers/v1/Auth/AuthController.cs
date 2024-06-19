using Business.Services.Auth.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.Auth;

[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthService _authService = ServiceTool.GetService<IAuthService>()!;

    #region login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(loginDto);

        if (result.HasFailed)
            return BadRequest(result);

        if (!result.ExtraData.Any())
            return Ok(result);

        return result.ExtraData.Any(extraData => extraData.Key == "useMFA" && (bool)extraData.Value!)
            ? StatusCode(428, result) // 428 Precondition Required
            : Ok(result);
    }
    #endregion

    #region register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(registerDto);

        if (result.HasFailed)
            return BadRequest(result);

        return Ok(result);
    }
    #endregion

    #region verify-mfa
    [HttpPost("verify-mfa")]
    public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaCodeDto verifyMfaCodeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.VerifyMfaCodeAsync(verifyMfaCodeDto);

        if (result.HasFailed)
            return BadRequest(result);

        return Ok(result);
    }
    #endregion

    #region forgot-password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);

        if (result.HasFailed)
            return BadRequest(result);

        return Ok(result);
    }
    #endregion

    #region logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(Guid userId)
    {
        var result = await _authService.Logout(userId);

        if (result.HasFailed)
            return BadRequest(result);

        return Ok(result);
    }
    #endregion
}