using Business.Services.ProjectManagement.Abstract;
using Core.Api.Abstract;
using Core.Constants;
using Core.Constants.SortOptions;
using Core.Utils.IoC;
using Domain.DTOs.ProjectManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.ProjectManagement;

[Authorize]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService = ServiceTool.GetService<ITeamService>()!;

    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll(TeamSortOptions? teamSortOptions)
    {
        var result = await _teamService.GetAllAsync(teamSortOptions);

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
    public async Task<IActionResult> GetTeamByManagerId(Guid id, TeamSortOptions? teamSortOptions)
    {
        var result = await _teamService.GetTeamByManagerIdAsync(id, teamSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetTeamByProjectId
    [HttpGet("project/{id:guid}")]
    public async Task<IActionResult> GetTeamByProjectId(Guid id, TeamSortOptions? teamSortOptions)
    {
        var result = await _teamService.GetTeamByProjectIdAsync(id, teamSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetTeamByName
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetTeamByName(string name, TeamSortOptions? teamSortOptions)
    {
        var result = await _teamService.GetTeamByNameAsync(name, teamSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region AssignUsersToTeam
    [HttpPost("assign-user")]
    public async Task<IActionResult> AssignUsersToTeam([FromBody] AssignUsersToTeamDto assignUsersToTeamDto)
    {
        var result = await _teamService.AssignUsersToTeamAsync(assignUsersToTeamDto);

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