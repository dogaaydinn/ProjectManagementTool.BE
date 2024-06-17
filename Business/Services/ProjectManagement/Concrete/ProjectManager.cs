using AutoMapper;
using Business.Services.ProjectManagement.Abstract;
using Core.Constants;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.IoC;
using Core.Utils.Rules;
using DataAccess.Repositories.Abstract.Association;
using DataAccess.Repositories.Abstract.ProjectManagement;
using DataAccess.Repositories.Abstract.TaskManagement;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Business.Services.ProjectManagement.Concrete;

public class ProjectManager : IProjectService
{
    private readonly IDutyDal _dutyDal = ServiceTool.GetService<IDutyDal>()!;
    private readonly IDutyProjectDal _dutyProjectDal = ServiceTool.GetService<IDutyProjectDal>()!;
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IProjectDal _projectDal = ServiceTool.GetService<IProjectDal>()!;
    private readonly ITeamProjectDal _teamProjectDal = ServiceTool.GetService<ITeamProjectDal>()!;
    private readonly IUserDal _userDal = ServiceTool.GetService<IUserDal>()!;
    private readonly IUserTeamDal _userTeamDal = ServiceTool.GetService<IUserTeamDal>()!;

    //marketing-created by elif,manager yusuf- tugba user, arda user,aslı user,ahmet user''project5
    //humanresorces created by ahmeet,manager emre-barıs user,yusuf user''project4,project8
    //operations created by onur, manager arda-adar user-adar user''project6
    //finance created by mehmet, onur user
    //sales adar-tugba user, arda user''project8,project1 
    //ai-created by tugba- doga manager- tugba user'' project1 
    //product development-adar-manager arda-doga user-adar user,ufuk user''project2
    //project management doga-manager adar-barıs user,onur user''project3
    //customer relations emre-manager yusuf
    //legal aslı,manager ahmet-
    //qualityAssurance selim-manager deniz
    //researchAndDevelopment fatih-manager cem
    //supplyChain burak-manager mehmet
    //logistics elif-manager tugba
    //warehousing onur-manager ufuk
    //informationTechnology adar
    //engineering doga-manager adar
    //design selim- manager doga
    //communications mehmet-manager tugba
    //financeAndAccounting emre-manager onur-ufuk user''project7,project2
    //research doga-

    #region GetAll

    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetAllAsync(ProjectSortOptions? projectSortOptions)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => !p.IsDeleted);
            var accessProjects = new HashSet<Project>();

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                accessProjects = projects.ToHashSet();
            }
            //oturum açmış kullanıcının bağlı olduğu ekiplerin projelerine ve kendisinin yöneticisi olduğu projelere erişim izni kontrol ediliyor.
            else
            {
                var loggedInUserId = AuthHelper.GetUserId()!.Value;

                var userTeams = await _userTeamDal.GetAllAsync(ut => ut.UserId == loggedInUserId);
                foreach (var userTeam in userTeams)
                {
                    //kullanıcının bağlı olduğu ekiplerin alınmasını sağlar.
                    var teamProjects = await _teamProjectDal.GetAllAsync(tp => tp.TeamId == userTeam.TeamId);
                    foreach (var teamProject in teamProjects)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == teamProject.ProjectId);
                        if (project != null) accessProjects.Add(project);
                    }
                }

                var userProjects = projects.Where(p => p.ManagerId == loggedInUserId);
                foreach (var userProject in userProjects) accessProjects.Add(userProject);
            }

            if (accessProjects.Count == 0)
            {
                result.SetError("PRJM-789658", "No projects found");
                return result;
            }

            var accessProjectsList = accessProjects.ToList();

            accessProjectsList = projectSortOptions switch
            {
                ProjectSortOptions.Name => accessProjectsList.OrderBy(p => p.Name).ToList(),
                ProjectSortOptions.Priority => accessProjectsList.OrderBy(p => p.Priority).ToList(),
                ProjectSortOptions.Status => accessProjectsList.OrderBy(p => p.Status).ToList(),
                ProjectSortOptions.DueDate => accessProjectsList.OrderBy(p => p.DueDate).ToList(),
                _ => accessProjectsList
            };

            var projectDto = _mapper.Map<List<ProjectGetDto>>(accessProjectsList);
            result.SetData(projectDto);
        }
        catch (ValidationException validationException)
        {
            result.Fail(validationException);
        }
        catch (Exception exception)
        {
            result.Fail(new ErrorMessage("PRJM-789658", exception.Message));
        }

        return result;
    }

    #endregion

    #region GetByIdAsync

    public async Task<ServiceObjectResult<ProjectGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var project = await _projectDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            // Check if the project exists
            BusinessRules.Run(
                ("PRJM-567953", BusinessRules.CheckEntityNull(project, "Project not found"))
            );
            var projectTeams = await _teamProjectDal.GetAllAsync(tp => tp.ProjectId == id);
            var hasAccess = false;
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                hasAccess = true;
            else if (project != null && project.ManagerId == loggedInUserId)
                hasAccess = true;
            else
                foreach (var projectTeam in projectTeams)
                {
                    var userTeams = await _userTeamDal.GetAllAsync(ut => ut.TeamId == projectTeam.TeamId);
                    if (userTeams.Any(userTeam => userTeam.UserId == loggedInUserId)) hasAccess = true;
                }

            // Check if the logged-in user has access to view the project
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-567953", "You do not have access to view this project"));
                return result;
            }

            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
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

    #region GetAllByManagerIdAsync

    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetAllByManagerIdAsync(Guid managerId)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            // Check if the logged-in user is the manager
            var hasAccess = loggedInUserId == managerId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-789658", "You do not have access to view this project"));
                return result;
            }

            // Get all projects managed by the given manager
            var projects = await _projectDal.GetAllAsync(p => p.ManagerId == managerId && !p.IsDeleted);

            if (!projects.Any())
            {
                result.SetError("PRJM-789658", "No projects found for the manager");
                return result;
            }

            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException validationException)
        {
            result.Fail(validationException);
        }
        catch (Exception exception)
        {
            result.Fail(new ErrorMessage("PRJM-789658", exception.Message));
        }

        return result;
    }

    #endregion

    #region GetProjectByPriorityAsync

    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByPriorityAsync(int priority)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            var projects = await _projectDal.GetAllAsync(p => (int)p.Priority == priority && p.IsDeleted == false);

            var accessProjects = new HashSet<Project>();

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                accessProjects = projects.ToHashSet();
            }
            else
            {
                var userTeams = await _userTeamDal.GetAllAsync(ut => ut.UserId == loggedInUserId);
                foreach (var userTeam in userTeams)
                {
                    var teamProjects = await _teamProjectDal.GetAllAsync(tp => tp.TeamId == userTeam.TeamId);
                    foreach (var teamProject in teamProjects)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == teamProject.ProjectId);
                        if (project != null) accessProjects.Add(project);
                    }
                }

                var userProjects = projects.Where(p => p.ManagerId == loggedInUserId);
                foreach (var userProject in userProjects) accessProjects.Add(userProject);
            }

            if (accessProjects.Count == 0)
            {
                result.SetError("PRJM-789658", "No projects found");
                return result;
            }

            var accessProjectsList = accessProjects.ToList();
            // Check if any projects were found
            BusinessRules.Run(
                ("PRJM-112134", BusinessRules.CheckCollectionNullOrEmpty(accessProjectsList, "No projects found"))
            );

            var projectDto = _mapper.Map<List<ProjectGetDto>>(accessProjectsList);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-112134", e.Message));
        }

        return result;
    }

    #endregion

    #region GetProjectByDutyId

    public async Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByDutyIdAsync(Guid dutyId)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(d => d.Id == dutyId && d.IsDeleted == false);

            // Check if the duty exists
            BusinessRules.Run(
                ("DUTY-123456", BusinessRules.CheckEntityNull(duty, "Duty not found"))
            );

            var project = await _projectDal.GetAsync(p => p.Id == duty!.ProjectId && p.IsDeleted == false);
            var projectTeams = await _teamProjectDal.GetAllAsync(tp => tp.ProjectId == project!.Id);
            var hasAccess = false;
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                hasAccess = true;
            else if (project != null && project.ManagerId == loggedInUserId)
                hasAccess = true;
            else
                foreach (var projectTeam in projectTeams)
                {
                    var userTeams = await _userTeamDal.GetAllAsync(ut => ut.TeamId == projectTeam.TeamId);
                    if (userTeams.Any(userTeam => userTeam.UserId == loggedInUserId)) hasAccess = true;
                }

            // Check if the logged-in user has access to view the project
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-567953", "You do not have access to view this project"));
                return result;
            }

            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
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

    #region GetProjectByName

    public async Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByNameAsync(string name)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            var project = await _projectDal.GetAsync(p => p.Name == name && p.IsDeleted == false);

            // Check if the project exists
            BusinessRules.Run(
                ("PRJM-958473", BusinessRules.CheckEntityNull(project, "Project not found"))
            );

            var projectTeams = await _teamProjectDal.GetAllAsync(tp => tp.ProjectId == project!.Id);
            var hasAccess = false;

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                hasAccess = true;
            else if (project != null && project.ManagerId == loggedInUserId)
                hasAccess = true;
            else
                foreach (var projectTeam in projectTeams)
                {
                    var userTeams = await _userTeamDal.GetAllAsync(ut => ut.TeamId == projectTeam.TeamId);
                    if (userTeams.Any(userTeam => userTeam.UserId == loggedInUserId)) hasAccess = true;
                }

            // Check if the logged-in user has access to view the project
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-958473", "You do not have access to view this project"));
                return result;
            }

            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }

    #endregion

    #region ChangeManagerOfExistingProject

    public async Task<ServiceObjectResult<ProjectGetDto?>> ChangeManagerOfExistingProject(Guid projectId,
        Guid newManagerId)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == projectId && p.IsDeleted == false);

            // Check if the project exists
            BusinessRules.Run(
                ("PRJM-537954", BusinessRules.CheckEntityNull(project, "Project not found"))
            );

            // Check if the logged-in user is the manager of the project or an admin
            var hasAccess = project!.ManagerId == loggedInUserId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-537954",
                    "You do not have access to change the manager of this project"));
                return result;
            }

            // Get the new manager
            var newManager = await _userDal.GetAsync(u => u.Id == newManagerId && u.IsDeleted == false);

            // Check if the new manager exists
            BusinessRules.Run(
                ("USER-537954", BusinessRules.CheckEntityNull(newManager, "New manager not found"))
            );

            project.ManagerId = newManagerId;
            await _projectDal.UpdateAsync(project);
            result.SetData(_mapper.Map<ProjectGetDto>(project));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-537954", e.Message));
        }

        return result;
    }

    #endregion

    #region Update

    public async Task<ServiceObjectResult<ProjectGetDto?>> UpdateAsync(ProjectUpdateDto projectUpdateDto)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == projectUpdateDto.Id && p.IsDeleted == false);

            // Check if the project exists
            BusinessRules.Run(
                ("PRJM-537954", BusinessRules.CheckEntityNull(project, "Project not found"))
            );

            var hasAccess = project!.ManagerId == loggedInUserId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-537954", "You do not have access to update this project"));
                return result;
            }

            _mapper.Map(projectUpdateDto, project);
            await _projectDal.UpdateAsync(project);
            result.SetData(_mapper.Map<ProjectGetDto>(project));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-537954", e.Message));
        }

        return result;
    }

    #endregion

    #region Create

    public async Task<ServiceObjectResult<ProjectGetDto>> CreateAsync(ProjectCreateDto projectCreateDto)
    {
        var result = new ServiceObjectResult<ProjectGetDto>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            var project = _mapper.Map<Project>(projectCreateDto);
            project.ManagerId = loggedInUserId; // Set the logged-in user as the manager
            project.CreatedUserId = loggedInUserId; // Set the logged-in user as the creator
            await _projectDal.AddAsync(project);
            result.SetData(_mapper.Map<ProjectGetDto>(project));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-538408", e.Message));
        }

        return result;
    }

    #endregion

    #region DeleteById

    public async Task<ServiceObjectResult<ProjectGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            // Get the project
            var project = await _projectDal.GetAsync(p => p.Id == id && p.IsDeleted == false);

            // Check if the project exists
            BusinessRules.Run(
                ("PRJM-537955", BusinessRules.CheckEntityNull(project, "Project not found"))
            );

            // Check if the logged-in user is the manager of the project or an admin
            var hasAccess = project!.ManagerId == loggedInUserId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("PRJM-537955", "You do not have access to delete this project"));
                return result;
            }

            await _projectDal.SoftDeleteAsync(project);
            result.Success("Project deleted successfully");
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-537955", e.Message));
        }

        return result;
    }

    #endregion

    #region GetProjectByStatusAsync

    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStatusAsync(int status)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => (int)p.Status == status && p.IsDeleted == false);

            var accessProjects = new HashSet<Project>();

            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                accessProjects = projects.ToHashSet();
            }
            else
            {
                var loggedInUserId = AuthHelper.GetUserId()!.Value;

                var userTeams = await _userTeamDal.GetAllAsync(ut => ut.UserId == loggedInUserId);
                foreach (var userTeam in userTeams)
                {
                    var teamProjects = await _teamProjectDal.GetAllAsync(tp => tp.TeamId == userTeam.TeamId);
                    foreach (var teamProject in teamProjects)
                    {
                        var project = projects.FirstOrDefault(p => p.Id == teamProject.ProjectId);
                        if (project != null) accessProjects.Add(project);
                    }
                }

                var userProjects = projects.Where(p => p.ManagerId == loggedInUserId);
                foreach (var userProject in userProjects) accessProjects.Add(userProject);
            }

            if (accessProjects.Count == 0)
            {
                result.SetError("PRJM-789658", "No projects found");
                return result;
            }

            var accessProjectsList = accessProjects.ToList();
            // Check if any projects were found
            BusinessRules.Run(
                ("PRJM-958473", BusinessRules.CheckCollectionNullOrEmpty(accessProjectsList, "No projects found"))
            );

            var projectDto = _mapper.Map<List<ProjectGetDto>>(accessProjectsList);
            result.SetData(projectDto);
        }
        catch (ValidationException validationException)
        {
            result.Fail(validationException);
        }
        catch (Exception exception)
        {
            result.Fail(new ErrorMessage("PRJM-958473", exception.Message));
        }

        return result;
    }

    public Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByPriorityAsync(int priority)
    {
        throw new NotImplementedException();
    }

    #endregion
}