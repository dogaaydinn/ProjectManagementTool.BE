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
public class ProjectController : BaseController
{
    private readonly IProjectService _projectService = ServiceTool.GetService<IProjectService>()!;

    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll(ProjectSortOptions? projectSortOptions)
    {
        var result = await _projectService.GetAllAsync(projectSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _projectService.GetByIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetAllByManagerId
    [HttpGet("manager/{id:guid}")]
    public async Task<IActionResult> GetAllByManagerId(Guid id,ProjectSortOptions? projectSortOptions)
    {
        var result = await _projectService.GetAllByManagerIdAsync(id, projectSortOptions);
        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetProjectByStatus
    [HttpGet("status/{status:int}")]
    public async Task<IActionResult> GetProjectByStatus(int status,ProjectSortOptions? projectSortOptions)
    {
        var result = await _projectService.GetProjectByStatusAsync(status, projectSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetProjectByPriority
    [HttpGet("priority/{priority:int}")]
    public async Task<IActionResult> GetProjectByPriority(int priority,ProjectSortOptions? projectSortOptions)
    {
        var result = await _projectService.GetProjectByPriorityAsync(priority, projectSortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetProjectByDutyId
    [HttpGet("duty/{id:guid}")]
    public async Task<IActionResult> GetProjectByDutyId(Guid id)
    {
        var result = await _projectService.GetProjectByDutyIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetProjectByName
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetProjectByName(string name)
    {
        var result = await _projectService.GetProjectByNameAsync(name);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region ChangeManagerOfExistingProject
    [HttpPut("change-manager")]
    public async Task<IActionResult> ChangeManagerOfExistingProject([FromBody] ChangeManagerOfExistingProjectDto changeManagerOfExistingProjectDto)
    {
        var result = await _projectService.ChangeManagerOfExistingProject(changeManagerOfExistingProjectDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region AssignTeamToProject
    [HttpPut("assign-team")]
    public async Task<IActionResult> AssignTeamToProject([FromBody] AssignTeamToProjectDto assignTeamToProjectDto)
    {
        var result = await _projectService.AssignTeamToProject(assignTeamToProjectDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    # region Update
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProjectUpdateDto projectUpdateDto)
    {
        var result = await _projectService.UpdateAsync(projectUpdateDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectCreateDto projectCreateDto)
    {
        var result = await _projectService.CreateAsync(projectCreateDto);

        if (result.HasFailed)
            return BadRequest(result);

        return Ok(result);
    }
    #endregion

    #region DeleteById
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _projectService.DeleteByIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion
}