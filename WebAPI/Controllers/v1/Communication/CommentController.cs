using AutoMapper;
using Business.Services.Communication.Abstract;
using Core.Api.Abstract;
using Core.Utils.IoC;
using Domain.DTOs.Communication;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1.Communication;

public class CommentController : BaseController
{
    private readonly ICommentService _commentService = ServiceTool.GetService<ICommentService>()!;
    
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _commentService.GetAllAsync();
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _commentService.GetByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region GetByDutyId
    [HttpGet("duty/{dutyId:guid}")]
    public async Task<IActionResult> GetByDutyId(Guid dutyId)
    {
        var result = await _commentService.GetByDutyIdAsync(dutyId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region GetByAuthorId
    [HttpGet("author/{authorId:guid}")]
    public async Task<IActionResult> GetByAuthorId(Guid authorId)
    {
        var result = await _commentService.GetByAuthorIdAsync(authorId);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region ReplyTo
      [HttpGet("{replyToId}")]
    public async Task<IActionResult> GetCommentsByReplyToId(Guid replyToId)
    {
        var result = await _commentService.GetByReplyToIdAsync(replyToId);
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CommentUpdateDto commentUpdateDto)
    {
        var result = await _commentService.UpdateAsync(commentUpdateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentCreateDto commentCreateDto)
    {
        var result = await _commentService.CreateAsync(commentCreateDto);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion
    
    #region DeleteById
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var result = await _commentService.DeleteByIdAsync(id);
        
        if (result.HasFailed) 
            return BadRequest();
        
        return Ok(result);
    }
    #endregion 
}