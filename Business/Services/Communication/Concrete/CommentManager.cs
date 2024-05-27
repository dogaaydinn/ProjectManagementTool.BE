using AutoMapper;
using Business.Services.Communication.Abstract;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.Communication;
using Domain.DTOs.Communication;
using Domain.Entities.Communication;

namespace Business.Services.Communication.Concrete;

public class CommentManager : ICommentService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly ICommentDal _commentDal = ServiceTool.GetService<ICommentDal>()!;
        
    
    #region GetAll
    public async Task<ServiceCollectionResult<CommentGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<CommentGetDto?>();

        try
        {
            var comments = await _commentDal.GetAllAsync(x => x.IsDeleted == false);
            var commentDto = _mapper.Map<List<CommentGetDto>>(comments);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-875909", e.Message));
        }

        return result;
    }
    #endregion

    #region GetById

    public async Task<ServiceObjectResult<CommentGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<CommentGetDto?>();

        try
        {
            var comment = await _commentDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            var commentDto = _mapper.Map<CommentGetDto>(comment);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-749030", e.Message));
        }
        
        return result;

    }


    #endregion

    #region GetByDutyId
    public async Task<ServiceCollectionResult<CommentGetDto?>> GetByDutyIdAsync(Guid dutyId)
    {
        var result = new ServiceCollectionResult<CommentGetDto?>();

        try
        {
            var comments = await _commentDal.GetAllAsync(x => x.DutyId == dutyId && x.IsDeleted == false);
            var commentDto = _mapper.Map<List<CommentGetDto>>(comments);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-875909", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByAuthorId
    public async Task<ServiceCollectionResult<CommentGetDto?>> GetByAuthorIdAsync(Guid authorId)
    {
        var result = new ServiceCollectionResult<CommentGetDto?>();

        try
        {
            var comments = await _commentDal.GetAllAsync(x => x.AuthorId == authorId && x.IsDeleted == false);
            var commentDto = _mapper.Map<List<CommentGetDto>>(comments);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-875909", e.Message));
        }

        return result;
    }
    #endregion

    #region ReplyTo
    public async Task<ServiceCollectionResult<CommentGetDto?>> GetByReplyToIdAsync(Guid replyToId)
    {
        var result = new ServiceCollectionResult<CommentGetDto?>();

        try
        {
            var comments = await _commentDal.GetAllAsync(x => x.ReplyToId == replyToId && x.IsDeleted == false);
            var commentDto = _mapper.Map<List<CommentGetDto>>(comments);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-875909", e.Message));
        }

        return result;
    }
    #endregion

    #region Update
    public async Task<ServiceObjectResult<CommentGetDto?>> UpdateAsync(CommentUpdateDto commentUpdateDto)
    {
        var result = new ServiceObjectResult<CommentGetDto?>();

        if (commentUpdateDto == null)
        {
            result.Fail(new ErrorMessage("Comment-875922", "CommentUpdateDto is null"));
            return result;
        }

        if (_commentDal == null || _mapper == null)
        {
            result.Fail(new ErrorMessage("Comment-875922", "_commentDal or _mapper is null"));
            return result;
        }

        try
        {
            var comment = await _commentDal.GetAsync(u => u.Id == commentUpdateDto.Id && u.IsDeleted == false);

            if (comment == null)
            {
                result.Fail(new ErrorMessage("Comment-875922", "Comment not found"));
                return result;
            }

            comment = _mapper.Map(commentUpdateDto, comment);
            await _commentDal.UpdateAsync(comment);
            var commentDto = _mapper.Map<CommentGetDto>(comment);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-232356", e.Message));
        }

        return result;
    }
    #endregion

    #region Create
    public async Task<ServiceObjectResult<CommentGetDto>> CreateAsync(CommentCreateDto commentCreateDto)
    {
        var result = new ServiceObjectResult<CommentGetDto>();

        try
        {
            var comment = _mapper.Map<Comment>(commentCreateDto);
            await _commentDal.AddAsync(comment);
            result.SetData(_mapper.Map<CommentGetDto>(comment));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-339459", e.Message));
        }
        return result;
    }
    #endregion
    
    #region DeleteById
    public async Task<ServiceObjectResult<CommentGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<CommentGetDto?>();

        try
        {
            var comment = await _commentDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (comment == null)
            {
                result.Fail(new ErrorMessage("Comment-875922", "Comment not found"));
                return result;
            }

            await _commentDal.SoftDeleteAsync(comment);
            var commentDto = _mapper.Map<CommentGetDto>(comment);
            result.SetData(commentDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("Comment-232356", e.Message));
        }

        return result;
    }
    #endregion
}