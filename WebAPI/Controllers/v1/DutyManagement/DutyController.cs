using Business.Services.DutyManagement.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.DutyManagement;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.DutyManagement;

public class DutyController : BaseController
{
    private readonly IDutyService _dutyService = ServiceTool.GetService<IDutyService>()!;
    
    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _dutyService.GetAllAsync();
        
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
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var result = await _dutyService.GetByUserIdAsync(userId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByTitle
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        var result = await _dutyService.GetByTitleAsync(title);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByProjectId
    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProjectId(Guid projectId)
    {
        var result = await _dutyService.GetByProjectIdAsync(projectId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetByStatus
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(int status)
    {
        var result = await _dutyService.GetByStatusAsync(status);

        if (result.HasFailed)
            return BadRequest();

        return Ok(result);
    }
    #endregion
    
    #region GetByPriority
    [HttpGet("priority/{priority}")]
    public async Task<IActionResult> GetByPriority(int priority)
    {
        var result = await _dutyService.GetByPriorityAsync(priority);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByReporterId
    [HttpGet("reporter/{reporterId:guid}")]
    public async Task<IActionResult> GetByReporterId(Guid reporterId)
    {
        var result = await _dutyService.GetByReporterIdAsync(reporterId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByAssigneeId
    [HttpGet("assignee/{assigneeId:guid}")]
    public async Task<IActionResult> GetByAssigneeId(Guid assigneeId)
    {
        var result = await _dutyService.GetByAssigneeIdAsync(assigneeId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByDutyType
    [HttpGet("dutyType/{dutyType}")]
    public async Task<IActionResult> GetByDutyType(int dutyType)
    {
        var result = await _dutyService.GetByDutyTypeAsync(dutyType);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByLabelId
    [HttpGet("label/{labelId:guid}")]
    public async Task<IActionResult> GetByLabelId(Guid labelId)
    {
        var result = await _dutyService.GetByLabelIdAsync(labelId);
        
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
    //TODO:AddAssigneeToDuty
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
}