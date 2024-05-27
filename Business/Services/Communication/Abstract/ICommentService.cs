using Core.Services;
using Core.Services.Result;
using Domain.DTOs.Communication;

namespace Business.Services.Communication.Abstract;

public interface ICommentService : IService
{
    Task<ServiceCollectionResult<CommentGetDto?>> GetAllAsync();
    Task<ServiceObjectResult<CommentGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<CommentGetDto?>> GetByDutyIdAsync(Guid dutyId);
    Task<ServiceCollectionResult<CommentGetDto?>> GetByAuthorIdAsync(Guid authorId);
    Task<ServiceCollectionResult<CommentGetDto?>> GetByReplyToIdAsync(Guid replyToId);
    Task<ServiceObjectResult<CommentGetDto?>> UpdateAsync(CommentUpdateDto commentUpdateDto);
    Task<ServiceObjectResult<CommentGetDto>> CreateAsync(CommentCreateDto commentCreateDto);
    Task<ServiceObjectResult<CommentGetDto?>> DeleteByIdAsync(Guid id);
}