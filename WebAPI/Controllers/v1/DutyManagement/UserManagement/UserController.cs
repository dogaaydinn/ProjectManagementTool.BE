using Business.Services.DutyManagement.Abstract;
using Core.Api.Abstract;
using Core.Constants;
using Core.Constants.AuthPolicies;
using Core.Utils.IoC;
using Domain.DTOs.DutyManagement.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.DutyManagement.UserManagement;

[ApiController]
public class UserController : BaseController
{
    private readonly IUserService _userService = ServiceTool.GetService<IUserService>()!;

    #region Get
    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        if (result.HasFailed)
            return BadRequest(result);
        return Ok(result);
    }
    #endregion

    #region GetById
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (result.HasFailed)
            return BadRequest(result);
        return Ok(result);
    }
    #endregion

    #region GetByRole
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    [HttpGet("role/{role}")]
    public async Task<IActionResult> GetByRole(string role)
    {
        var result = await _userService.GetByRoleAsync(role);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByUsername
    [Authorize]
    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var result = await _userService.GetByUsernameAsync(username);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByEmail
    [Authorize]
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _userService.GetByEmailAsync(email);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByPhoneNumber
    [Authorize]
    [HttpGet("phone/{phone}")]
    public async Task<IActionResult> GetByPhoneNumber(string phone)
    {
        var result = await _userService.GetByPhoneNumberAsync(phone);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region ChangePassword
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.ChangePasswordAsync(changePasswordDto);

        return result.HasFailed
            ? BadRequest(result)
            : Ok(result);
    }
    #endregion

    #region Update

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto userUpdateDto)
    {
        var result = await _userService.UpdateAsync(userUpdateDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion
    
    //TODO:RemoveUserFromTeam
    //TODO:RemoveUserFromProject
    // TODO: GetALlUsersByTeamId
    // TODO: GetALlUsersByProjectId

    #region DeleteById
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _userService.DeleteByIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion
}