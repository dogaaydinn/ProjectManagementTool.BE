using AutoMapper;
using Business.Services.DutyManagement.Abstract;
using Core.Constants;
using Core.Constants.Duty;
using Core.Constants.SortOptions;
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
    private readonly IUserTeamDal _userTeamDal = ServiceTool.GetService<IUserTeamDal>()!;
    private readonly IDutyAccessDal _dutyAccessDal = ServiceTool.GetService<IDutyAccessDal>()!;
    
    //adar bana .net user secret göstericek
    //sort option
    //debug
    //project teame duty ekleme
    //subduty parent duty
    
    #region GetAll
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetAllAsync(DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var accessDutiesList = await _dutyDal.GetAllAsync(d => d.IsDeleted == false);

            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };

            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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
    // TODO: Debug gerekli
    
    #region GetById
    public async Task<ServiceObjectResult<DutyGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(d => d.Id == id && d.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Duty not found"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId();        
            var hasAccess = false;

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                hasAccess = true;
            }
            else
            {
                var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && x.DutyId == duty.Id);
                var teamProjects = dutyAccesses.Select(x => x.TeamProject).ToList();
                var teamIds = teamProjects.Select(x => x.TeamId).ToList();

                foreach (var teamId in teamIds)
                {
                    var userTeams = await _userTeamDal.GetAllAsync(x =>
                        x.UserId == loggedInUserId && x.IsDeleted == false && x.TeamId == teamId);
                    if (userTeams.Count <= 0) continue;
                    hasAccess = true;
                    break;
                }
            }

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Access denied"));
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
    
    #region GetByReporterId
        public async Task<ServiceCollectionResult<DutyGetDto?>> GetByReporterIdAsync(Guid reporterId, DutySortOptions? dutySortOptions)
        {
            var result = new ServiceCollectionResult<DutyGetDto?>();

            try
            {
                var duties = await _dutyDal.GetAllAsync(d => d.ReporterId == reporterId && d.IsDeleted == false);
                var accessDuties = duties;
                
                var accessDutiesList = accessDuties.ToList();
                // Apply sorting
                accessDutiesList = dutySortOptions switch
                {
                    DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                    DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                    DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                    DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                    DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                    DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                    _ => accessDutiesList
                };
                
                var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    #region GetByManagerId
     public async Task<ServiceCollectionResult<DutyGetDto?>> GetByManagerIdAsync(DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            
            var projects = await _projectDal.GetAllAsync(p => p.ManagerId == loggedInUserId && p.IsDeleted == false);
            var projectIds = projects.Select(p => p.Id).ToList();
            var duties = await _dutyDal.GetAllAsync(d => projectIds.Contains(d.ProjectId) && d.IsDeleted == false);
            var accessDuties = duties;
            
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    #region GetByAssigneeId

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByAssigneeIdAsync(Guid assigneeId,DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {  var loggedInUserId = AuthHelper.GetUserId()!.Value;
        
            var userDuties = await _userDutyDal.GetAllAsync(x => x.UserId == assigneeId && x.IsDeleted == false);
            var dutyIds = userDuties.Select(d => d.DutyId).ToList();
            var duties = await _dutyDal.GetAllAsync(d => dutyIds.Contains(d.Id) && d.IsDeleted == false);
            var accessDuties = duties;
        
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
        
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByManagerIdAsync(Guid managerId, DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.ManagerId == managerId && p.IsDeleted == false);
            var projectIds = projects.Select(p => p.Id).ToList();
            var duties = await _dutyDal.GetAllAsync(d => projectIds.Contains(d.ProjectId) && d.IsDeleted == false);
            var accessDuties = duties;
            
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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
    
    #region GetByTitle
    public async Task<ServiceObjectResult<DutyGetDto?>> GetByTitleAsync(string title)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(d => d.Title == title && d.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Duty not found"));
                return result;
            }

            var loggedInUserId = AuthHelper.GetUserId();        
            var hasAccess = false;

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                hasAccess = true;
            }
            else
            {
                var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && x.DutyId == duty.Id);
                var teamProjects = dutyAccesses.Select(x => x.TeamProject).ToList();
                var teamIds = teamProjects.Select(x => x.TeamId).ToList();

                foreach (var teamId in teamIds)
                {
                    var userTeams = await _userTeamDal.GetAllAsync(x =>
                        x.UserId == loggedInUserId && x.IsDeleted == false && x.TeamId == teamId);
                    if (userTeams.Count <= 0) continue;
                    hasAccess = true;
                    break;
                }
                
            }
            
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Access denied"));
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
            result.Fail(new ErrorMessage("PRJM-567953", e.Message));
        }

        return result;
    }
    #endregion
    
    #region GetByProjectId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByProjectIdAsync(Guid projectId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            
            // If logged in user id is not admin or pm set hasAccess to false
            var project = await _projectDal.GetAsync(x => x.Id == projectId && x.IsDeleted == false);
            
            if (project == null)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Project not found"));
                return result;
            }
            
            if (!AuthHelper.GetRole()!.Equals(UserRoles.Admin) && project.ManagerId != loggedInUserId)
            {
                result.Fail(new ErrorMessage("DUTY-432894", "Access denied"));
                return result;
            }
            
            var teamProjects = await _teamProjectDal.GetAllAsync(x => x.ProjectId == projectId && x.IsDeleted == false);
            var teamProjectsId = teamProjects.Select(x => x.Id).ToList();
            var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && teamProjectsId.Contains(x.TeamProjectId));
            var dutyIds = dutyAccesses.Select(x => x.DutyId).ToList();
            var duties = await _dutyDal.GetAllAsync(x => dutyIds.Contains(x.Id) && x.IsDeleted == false);
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
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
    
    #region GetByStatus
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByStatusAsync(DutyStatus status,DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var duties = await _dutyDal.GetAllAsync(d => d.Status == status && d.IsDeleted == false);
            var accessDuties = new HashSet<Duty>();
            
            if(AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                accessDuties = duties.ToHashSet();
            else
            {
                foreach (var duty in duties)
                {
                    var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && x.DutyId == duty.Id);
                    var teamProjects = dutyAccesses.Select(x => x.TeamProject).ToList();
                    var teamIds = teamProjects.Select(x => x.TeamId).ToList();

                    foreach (var teamId in teamIds)
                    {
                        var userTeams = await _userTeamDal.GetAllAsync(x =>
                            x.UserId == loggedInUserId && x.IsDeleted == false && x.TeamId == teamId);
                        if (userTeams.Count <= 0) continue;
                        accessDuties.Add(duty);
                        break;
                    }
                }
            }
            
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    #region GetByPriority
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByPriorityAsync(Priority priority,DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var duties = await _dutyDal.GetAllAsync(d => d.Priority == priority && d.IsDeleted == false);
            var accessDuties = new HashSet<Duty>();
            
            if(AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                accessDuties = duties.ToHashSet();
            else
            {
                foreach (var duty in duties)
                {
                    var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && x.DutyId == duty.Id);
                    var teamProjects = dutyAccesses.Select(x => x.TeamProject).ToList();
                    var teamIds = teamProjects.Select(x => x.TeamId).ToList();

                    foreach (var teamId in teamIds)
                    {
                        var userTeams = await _userTeamDal.GetAllAsync(x =>
                            x.UserId == loggedInUserId && x.IsDeleted == false && x.TeamId == teamId);
                        if (userTeams.Count <= 0) continue;
                        accessDuties.Add(duty);
                        break;
                    }
                }
            }
            
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    #region GetByDutyType
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByDutyTypeAsync(DutyType dutyType,DutySortOptions? dutySortOptions)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var duties = await _dutyDal.GetAllAsync(d => d.DutyType == dutyType && d.IsDeleted == false);
            var accessDuties = new HashSet<Duty>();
            
            if(AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                accessDuties = duties.ToHashSet();
            else
            {
                foreach (var duty in duties)
                {
                    var dutyAccesses = await _dutyAccessDal.GetAllAsync(x => x.IsDeleted == false && x.DutyId == duty.Id);
                    var teamProjects = dutyAccesses.Select(x => x.TeamProject).ToList();
                    var teamIds = teamProjects.Select(x => x.TeamId).ToList();

                    foreach (var teamId in teamIds)
                    {
                        var userTeams = await _userTeamDal.GetAllAsync(x =>
                            x.UserId == loggedInUserId && x.IsDeleted == false && x.TeamId == teamId);
                        if (userTeams.Count <= 0) continue;
                        accessDuties.Add(duty);
                        break;
                    }
                }
            }
            
            var accessDutiesList = accessDuties.ToList();
            // Apply sorting
            accessDutiesList = dutySortOptions switch
            {
                DutySortOptions.Title => accessDutiesList.OrderBy(d => d.Title).ToList(),
                DutySortOptions.Priority => accessDutiesList.OrderBy(d => d.Priority).ToList(),
                DutySortOptions.Status => accessDutiesList.OrderBy(d => d.Status).ToList(),
                DutySortOptions.DueDate => accessDutiesList.OrderBy(d => d.DueDate).ToList(),
                DutySortOptions.Assignee => accessDutiesList.OrderBy(d => d.AssignedUsers?.Count ?? 0).ToList(),
                DutySortOptions.Reporter => accessDutiesList.OrderBy(d => d.ReporterId).ToList(),
                _ => accessDutiesList
            };
            
            var dutyDto = _mapper.Map<List<DutyGetDto>>(accessDutiesList);
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

    #region Update

    public async Task<ServiceObjectResult<DutyGetDto>> UpdateAsync(DutyUpdateDto dutyUpdateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {

            var duty = await _dutyDal.GetAsync(u => u.Id == dutyUpdateDto.Id && u.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-123456", "Duty not found"));
                return result;
            }
            
            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == duty.ProjectId);
            
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

    #region Create

    public async Task<ServiceObjectResult<DutyGetDto>> CreateAsync(DutyCreateDto dutyCreateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto>();

        try
        {
            
            var project = await _projectDal.GetAsync(p => p.Id == dutyCreateDto.ProjectId);

            // Check if the project exists
            if (project == null)
            {
                result.Fail(new ErrorMessage("DUTY-453356", "Project not found"));
                return result;
            }
            
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

            var duty = await _dutyDal.GetAsync(d => d.Id == id && d.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-537955", "Duty not found"));
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