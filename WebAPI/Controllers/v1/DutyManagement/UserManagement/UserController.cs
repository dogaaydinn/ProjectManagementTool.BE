using Business.Services.DutyManagement.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.Auth;
using Domain.DTOs.DutyManagement.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.DutyManagement.UserManagement;
[ApiController]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService = ServiceTool.GetService<IUserService>()!;

    #region Get
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetById
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByRole
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
    [HttpGet("phone/{phone}")]
    public async Task<IActionResult> GetByPhoneNumber(string phone)
    {
        var result = await _userService.GetByPhoneNumberAsync(phone);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region ResetPassword
    [HttpPost("change-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.ResetPasswordAsync(resetPasswordDto);

        return result.HasFailed
            ? BadRequest(result)
            : Ok(result);
    }
    #endregion
    
    #region Update
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto userUpdateDto)
    {
        var result = await _userService.UpdateAsync(userUpdateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    //TODO:AssignUserToProject
    //TODO:AssignUserToTeam
    //TODO:AssignUserToRole
    //TODO:RemoveUserFromTeam
    //TODO:RemoveUserFromProject
    //TODO:ChangeUserRole
    //TODO:ChangeUserStatus

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto userGetDto)
    {
        var result = await _userService.CreateAsync(userGetDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region DeleteById
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

