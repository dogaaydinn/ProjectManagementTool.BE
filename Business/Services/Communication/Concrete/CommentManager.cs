using AutoMapper;
using Business.Services.Communication.Abstract;
using Core.Constants;
using Core.Constants.SortOptions;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.Association;
using DataAccess.Repositories.Abstract.Communication;
using DataAccess.Repositories.Abstract.ProjectManagement;
using DataAccess.Repositories.Abstract.TaskManagement;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.Communication;
using Domain.Entities.Communication;

namespace Business.Services.Communication.Concrete;

public class CommentManager : ICommentService
{
    private readonly ICommentDal _commentDal = ServiceTool.GetService<ICommentDal>()!;
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IDutyAccessDal _dutyAccessDal = ServiceTool.GetService<IDutyAccessDal>()!;
    private readonly ITeamProjectDal _teamProjectDal = ServiceTool.GetService<ITeamProjectDal>()!;
    private readonly IUserTeamDal _userTeamDal = ServiceTool.GetService<IUserTeamDal>()!;
    private readonly ITeamDal _teamDal = ServiceTool.GetService<ITeamDal>()!;
    private readonly IUserDal _userDal = ServiceTool.GetService<IUserDal>()!;
    private readonly IDutyDal _dutyDal = ServiceTool.GetService<IDutyDal>()!;

    #region GetAll
    public async Task<ServiceCollectionResult<CommentGetDto?>> GetAllAsync(DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<CommentGetDto?>();

        try
        {
            var comments = await _commentDal.GetAllAsync(x => x.IsDeleted == false);
            var accessComments = new HashSet<Comment>();

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                // convert ICollection<Comment> to HashSet<Comment>
                accessComments = new HashSet<Comment>(comments);
            }
            else
            {
                /*
                var loggedInUserId = AuthHelper.GetUserId()!.Value;
                var userTeams = await _userTeamDal.GetAllAsync(x => x.UserId == loggedInUserId && x.IsDeleted == false);
                var allTeamProjects = await _teamProjectDal.GetAllAsync(x => x.IsDeleted == false);
                var userTeamProjects = allTeamProjects.Where(x => userTeams.Any(y => y.TeamId == x.TeamId)).ToList();
                var allDutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false);
                var userDutyAccesses = allDutyAccesses.Where(x => userTeamProjects.Any(y => y.Id == x.TeamProjectId)).ToList();
                IList<Guid> dutyIds = userDutyAccesses.Select(x => x.DutyId).ToList();

                var duties = await _dutyDal.GetAllAsync(x => dutyIds.Contains(x.Id) && x.IsDeleted == false);

                foreach (var duty in duties)
                {
                    var xComments = await _commentDal.GetAllAsync(x => x.DutyId == duty.Id && x.IsDeleted == false);
                    
                    foreach (var comment in xComments)
                        accessComments.Add(comment);
                }
                */
            }
            
            
            var accessCommentsList = accessComments.ToList();
            accessCommentsList = dutySortOptions switch
            {
                DutySortOptions.Title => accessCommentsList.OrderBy(x => x.Duty.Title).ToList(),
                DutySortOptions.Status => accessCommentsList.OrderBy(x => x.Duty.Status).ToList(),
                DutySortOptions.Priority => accessCommentsList.OrderBy(x => x.Duty.Priority).ToList(),
                DutySortOptions.Reporter => accessCommentsList.OrderBy(x => x.Duty.ReporterId).ToList(),
                DutySortOptions.DueDate => accessCommentsList.OrderBy(x => x.Duty.DueDate).ToList(),
                _ => accessCommentsList
            };

            var commentDto = _mapper.Map<List<CommentGetDto>>(accessCommentsList);
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
            var commentDtos = _mapper.Map<List<CommentGetDto>>(comments);
            result.SetData(commentDtos);
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

    // TODO: Yanlış
    public async Task<ServiceObjectResult<CommentGetDto?>> UpdateAsync(CommentUpdateDto commentUpdateDto)
    {
        var result = new ServiceObjectResult<CommentGetDto?>();

        // Check if the commentUpdateDto is null
        if (commentUpdateDto == null)
        {
            result.Fail(new ErrorMessage("Comment-875922", "CommentUpdateDto is null"));
            return result;
        }

        // Check if _commentDal or _mapper are null
        if (_commentDal == null || _mapper == null)
        {
            result.Fail(new ErrorMessage("Comment-875922", "_commentDal or _mapper is null"));
            return result;
        }

        try
        {
            // Retrieve the existing comment
            var comment = await _commentDal.GetAsync(u => u.Id == commentUpdateDto.Id && u.IsDeleted == false);

            // Check if the comment exists
            if (comment == null)
            {
                result.Fail(new ErrorMessage("Comment-875922", "Comment not found"));
                return result;
            }

            // Map the updated properties from commentUpdateDto to the existing comment
            _mapper.Map(commentUpdateDto, comment);

            // Update the comment in the database
            await _commentDal.UpdateAsync(comment);

            // Map the updated comment to CommentGetDto
            var commentDto = _mapper.Map<CommentGetDto>(comment);

            // Set the data and return the result
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

    public async Task<ServiceObjectResult<CommentGetDto?>> CreateAsync(CommentCreateDto commentCreateDto)
    {
        var result = new ServiceObjectResult<CommentGetDto?>();

        try
        {
            var comment = _mapper.Map<Comment>(commentCreateDto);

            // Check if the comment is a reply to another comment
            if (commentCreateDto.ReplyToId.HasValue)
            {
                // Ensure the parent comment exists
                var parentComment = await _commentDal.GetByIdAsync(commentCreateDto.ReplyToId.Value);
                if (parentComment == null)
                {
                    result.Fail(new ErrorMessage("Comment-404", "The parent comment does not exist."));
                    return result;
                }
            }

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