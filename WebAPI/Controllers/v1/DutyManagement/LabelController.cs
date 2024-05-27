using Business.Services.DutyManagement.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.DutyManagement;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.DutyManagement;

public class LabelController : BaseController
{
    private readonly ILabelService _labelService = ServiceTool.GetService<ILabelService>()!;
    
    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _labelService.GetAllAsync();
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _labelService.GetByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetLabelsByColor
    [HttpGet("color/{color}")]
    public async Task<IActionResult> GetLabelsByColor(string color)
    {
        var result = await _labelService.GetByColorAsync(color);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region Update
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] LabelUpdateDto labelUpdateDto)
    {
        var result = await _labelService.UpdateAsync(labelUpdateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    //TODO:GetLabelByProjectId
    
    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LabelCreateDto labelCreateDto)
    {
        var result = await _labelService.CreateAsync(labelCreateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region DeleteById
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _labelService.DeleteByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion 
}