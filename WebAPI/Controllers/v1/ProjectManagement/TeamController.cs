using Business.Services.ProjectManagement.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.ProjectManagement;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.ProjectManagement;

public class TeamController : BaseController
{
    private readonly ITeamService _teamService = ServiceTool.GetService<ITeamService>()!;

    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _teamService.GetAllAsync();
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _teamService.GetByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetTeamByManagerId
    [HttpGet("manager/{id:guid}")]
    public async Task<IActionResult> GetTeamByManagerId(Guid id)
    {
        var result = await _teamService.GetTeamByManagerIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetTeamByProjectId
    [HttpGet("project/{id:guid}")]
    public async Task<IActionResult> GetTeamByProjectId(Guid id)
    {
        var result = await _teamService.GetTeamByProjectIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetTeamByName
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetTeamByName(string name)
    {
        var result = await _teamService.GetTeamByNameAsync(name);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetTeamByStatus
    [HttpGet("status/{status:int}")]
    public async Task<IActionResult> GetTeamByStatus(int status)
    {
        var result = await _teamService.GetTeamByStatusAsync(status);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetTeamByPriority
    [HttpGet("priority/{priority:int}")]
    public async Task<IActionResult> GetTeamByPriority(int priority)
    {
        var result = await _teamService.GetTeamByPriorityAsync(priority);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TeamUpdateDto teamUpdateDto)
    {
        var result = await _teamService.UpdateAsync(teamUpdateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeamCreateDto teamCreateDto)
    {
        var result = await _teamService.CreateAsync(teamCreateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region DeleteById
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _teamService.DeleteByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion 
}