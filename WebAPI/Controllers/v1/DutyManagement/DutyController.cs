using Business.Services.DutyManagement.Abstract;
using Core.Api.Abstract;
using Core.Constants;
using Core.Constants.Duty;
using Core.Utils.IoC;
using Domain.DTOs.DutyManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.DutyManagement;

[Authorize]
public class DutyController : BaseController
{
    private readonly IDutyService _dutyService = ServiceTool.GetService<IDutyService>()!;
/*
    // TODO: Duty için sıralama seçenekleri eklenecek.
    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll(DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetAllAsync(dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetById

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _dutyService.GetByIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion

    #region GetByUserId
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByUserIdAsync(userId, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByTitle
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByTitleAsync(title, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByProjectId
    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProjectId(Guid projectId, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByProjectIdAsync(projectId, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByStatus
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(DutyStatus status, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByStatusAsync(status, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByPriority
    [HttpGet("priority/{priority}")]
    public async Task<IActionResult> GetByPriority(Priority priority, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByPriorityAsync(priority, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByReporterId
    [HttpGet("reporter/{reporterId:guid}")]
    public async Task<IActionResult> GetByReporterId(Guid reporterId, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByReporterIdAsync(reporterId, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByAssigneeId
    [HttpGet("assignee/{assigneeId:guid}")]
    public async Task<IActionResult> GetByAssigneeId(Guid assigneeId, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByAssigneeIdAsync(assigneeId, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    #region GetByDutyType
    [HttpGet("dutyType/{dutyType}")]
    public async Task<IActionResult> GetByDutyType(DutyType dutyType, DutySortOptions? dutySortOptions)
    {
        var result = await _dutyService.GetByDutyTypeAsync(dutyType, dutySortOptions);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion

    // TODO: RemoveDTO yaz
    #region RemoveDutyFromProject

    [HttpDelete("project/{projectId:guid}")]
    public async Task<IActionResult> RemoveDutyFromProject([FromBody] RemoveDutyFromProjectDto removeDutyFromProjectDto)
    {
        var result = await _dutyService.RemoveDutyFromProjectAsync(removeDutyFromProjectDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion

    #region Update

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DutyUpdateDto dutyUpdateDto)
    {
        var result = await _dutyService.UpdateAsync(dutyUpdateDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion

    //TODO:AssignDutyToProject
    //TODO:AddAssigneeToDuty (Assign user to duty)
    //TODO:RemoveAssigneeFromDuty
    
    #region Create

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DutyCreateDto dutyCreateDto)
    {
        var result = await _dutyService.CreateAsync(dutyCreateDto);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion

    #region DeleteById

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _dutyService.DeleteByIdAsync(id);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }

    #endregion
    */
}