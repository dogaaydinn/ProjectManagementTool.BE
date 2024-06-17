using AutoMapper;
using Business.Services.Auth.Concrete;
using Business.Services.DutyManagement.Abstract;
using Core.Constants;
using Core.Constants.Duty;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.Association;
using DataAccess.Repositories.Abstract.ProjectManagement;
using DataAccess.Repositories.Abstract.TaskManagement;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.DutyManagement;
using Domain.Entities.DutyManagement;

namespace Business.Services.DutyManagement.Concrete;

public class DutyManager : IDutyService
{
    private readonly IDutyDal _dutyDal = ServiceTool.GetService<IDutyDal>()!;
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IProjectDal _projectDal = ServiceTool.GetService<IProjectDal>()!;
    private readonly IUserDal _userDal = ServiceTool.GetService<IUserDal>()!;
    private readonly ITeamProjectDal _teamProjectDal = ServiceTool.GetService<ITeamProjectDal>()!;
    private readonly IUserDutyDal _userDutyDal = ServiceTool.GetService<IUserDutyDal>()!;
    
    #region GetAll

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            
            var dutySet = new HashSet<Duty>();
            bool hasAccess;
            
            if(AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {            
                var duties = await _dutyDal.GetAllAsync(d => d.IsDeleted == false);
                dutySet = duties.ToHashSet();
            }
            else
            {
                // Get all dutyes that belong to the projject if the user is the manager of the project
                var userDuties = await _userDutyDal.GetAllAsync(ud => ud.UserId == loggedInUserId);
            }
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-789658", e.Message));
        }

        return result;
    }

    #endregion

    //TODO: parentduty, subduty

    //tamam

    #region GetById

    public async Task<ServiceObjectResult<DutyGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(d => d.Id == id && d.IsDeleted == false);
            var loggedInUserId = AuthHelper.GetUserId();

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Duty not found"));
                return result;
            }

            // Check if the logged in user is the manager of the project that the duty belongs to,
            // or if they are the reporter and creator of the duty,
            // or if they are an assignee of the duty.
            if (!(duty.Project.ManagerId == loggedInUserId ||
                  (duty.ReporterId == loggedInUserId && duty.ReporterId == loggedInUserId) ||
                  duty.AssignedUsers.Any(u => u.Id == loggedInUserId)))
            {
                result.Fail(new ErrorMessage("DUTY-432894", "You do not have access to this duty"));
                return result;
            }

            var dutyDto = _mapper.Map<DutyGetDto>(duty);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-432894", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByUserId

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByUserIdAsync(Guid userId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-328576", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            if (loggedInUserId != userId)
            {
                result.Fail(new ErrorMessage("DUTY-328576", "Unauthorized access"));
                return result;
            }

            var duties = await _dutyDal.GetAllAsync(d => d.IsDeleted == false &&
                                                         ((d.ReporterId == userId && d.ReporterId == userId) ||
                                                          d.Project.ManagerId == userId ||
                                                          d.AssignedUsers.Any(u => u.Id == userId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-328576", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByTitle

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByTitleAsync(string title)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.Title == title && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByProjectId

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByProjectIdAsync(Guid projectId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.ProjectId == projectId && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByStatus

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByStatusAsync(DutyStatus status)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.Status == status && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByPriority

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByPriorityAsync(Priority priority)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.Priority == priority && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByReporterId

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByReporterIdAsync(Guid reporterId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.ReporterId == reporterId && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region GetByAssigneeId

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByAssigneeIdAsync(Guid assigneeId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d =>
                d.AssignedUsers.Any(u => u.Id == assigneeId) && d.IsDeleted == false &&
                ((d.ReporterId == loggedInUserId && d.ReporterId == loggedInUserId) ||
                 d.Project.ManagerId == loggedInUserId ||
                 d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam <3

    #region GetByDutyType

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByDutyTypeAsync(DutyType dutyType)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-212122", "User is not logged in"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            var duties = await _dutyDal.GetAllAsync(d => d.DutyType == dutyType && d.IsDeleted == false &&
                                                         ((d.ReporterId == loggedInUserId &&
                                                           d.ReporterId == loggedInUserId) ||
                                                          d.Project.ManagerId == loggedInUserId ||
                                                          d.AssignedUsers.Any(u => u.Id == loggedInUserId)));
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region RemoveDutyFromProject

    public async Task<ServiceObjectResult<bool>> RemoveDutyFromProjectAsync(Guid projectId, Guid dutyId)
    {
        var result = new ServiceObjectResult<bool>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-537954", "User is not logged in"));
                return result;
            }

            var project = await _projectDal.GetAsync(p => p.Id == projectId && p.IsDeleted == false);
            var duty = await _dutyDal.GetAsync(d => d.Id == dutyId && d.IsDeleted == false);

            if (project == null)
            {
                result.Fail(new ErrorMessage("DUTY-537954", "Project not found"));
                return result;
            }

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-537954", "Duty not found"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            if (project.ManagerId != loggedInUserId && duty.ReporterId != loggedInUserId)
            {
                result.Fail(new ErrorMessage("DUTY-537954",
                    "You do not have access to remove this duty from the project"));
                return result;
            }

            project.Duties.Remove(duty);
            await _projectDal.UpdateAsync(project);

            result.SetData(true);
            result.Success("Duty removed from project successfully");
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-537954", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region Update

    public async Task<ServiceObjectResult<DutyGetDto?>> UpdateAsync(DutyUpdateDto dutyUpdateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-123456", "User is not logged in"));
                return result;
            }

            var duty = await _dutyDal.GetAsync(u => u.Id == dutyUpdateDto.Id && u.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-123456", "Duty not found"));
                return result;
            }

            // Get the logged in user's id
            var loggedInUserId = AuthHelper.GetUserId().Value;

            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == duty.ProjectId);

            // Check if the logged in user is the project manager or the reporter of the duty
            if (duty.ReporterId != loggedInUserId && project.ManagerId != loggedInUserId)
            {
                result.Fail(new ErrorMessage("DUTY-123456", "You do not have access to update this duty"));
                return result;
            }

            // Map the dutyUpdateDto to the existing duty
            _mapper.Map(dutyUpdateDto, duty);

            // Update the duty in the database
            await _dutyDal.UpdateAsync(duty);

            // Map the updated duty to a DutyGetDto
            var dutyDto = _mapper.Map<DutyGetDto>(duty);

            // Set the result data and return a success message
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-123456", e.Message));
        }

        return result;
    }

    #endregion

    //tamam

    #region Create

    public async Task<ServiceObjectResult<DutyGetDto>> CreateAsync(DutyCreateDto dutyCreateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto>();

        try
        {
            // Check if the user is logged in
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-453356", "User is not logged in"));
                return result;
            }

            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == dutyCreateDto.ProjectId);

            // Check if the project exists
            if (project == null)
            {
                result.Fail(new ErrorMessage("DUTY-453356", "Project not found"));
                return result;
            }

            // Get the logged in user's id
            var loggedInUserId = AuthHelper.GetUserId().Value;

            // Check if the logged in user is a member of the project
            if (project.Users.All(m => m.Id != loggedInUserId))
            {
                result.Fail(new ErrorMessage("DUTY-453356", "You are not a member of this project"));
                return result;
            }

            // Map the dutyCreateDto to a Duty entity
            var duty = _mapper.Map<Duty>(dutyCreateDto);

            // Add the duty to the database
            await _dutyDal.AddAsync(duty);

            // Map the Duty entity to a DutyGetDto
            var dutyDto = _mapper.Map<DutyGetDto>(duty);

            // Set the result data and return a success message
            result.SetData(dutyDto);
            result.Success("Duty created successfully");
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-453356", e.Message));
        }

        return result;
    }

    #endregion

    //30 gün sonra hard delete

    #region DeleteById

    public async Task<ServiceObjectResult<DutyGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            if (!AuthHelper.IsLoggedIn())
            {
                result.Fail(new ErrorMessage("DUTY-537955", "User is not logged in"));
                return result;
            }

            var duty = await _dutyDal.GetAsync(d => d.Id == id && d.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-537955", "Duty not found"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId().Value;

            if (duty.ReporterId != loggedInUserId && duty.Project.ManagerId != loggedInUserId)
            {
                result.Fail(new ErrorMessage("DUTY-537955", "You do not have access to delete this duty"));
                return result;
            }

            // Soft delete the duty
            duty.IsDeleted = true;
            duty.DeletedAt = DateTime.UtcNow;

            await _dutyDal.UpdateAsync(duty);
            result.Success("Duty deleted successfully");
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-537955", e.Message));
        }

        return result;
    }

    #endregion
}