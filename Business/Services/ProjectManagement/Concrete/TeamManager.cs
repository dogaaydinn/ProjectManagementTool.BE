using AutoMapper;
using Business.Services.ProjectManagement.Abstract;
using Core.Constants;
using Core.Constants.SortOptions;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.IoC;
using Core.Utils.Rules;
using DataAccess.Repositories.Abstract.Association;
using DataAccess.Repositories.Abstract.ProjectManagement;
using Domain.DTOs.ProjectManagement;
using Domain.Entities.Association;
using Domain.Entities.ProjectManagement;

namespace Business.Services.ProjectManagement.Concrete;

public class TeamManager : ITeamService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IProjectDal _projectDal = ServiceTool.GetService<IProjectDal>()!;
    private readonly ITeamDal _teamDal = ServiceTool.GetService<ITeamDal>()!;
    private readonly ITeamProjectDal _teamProjectDal = ServiceTool.GetService<ITeamProjectDal>()!;
    private readonly IUserTeamDal _userTeamDal = ServiceTool.GetService<IUserTeamDal>()!;

    #region GetAll
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetAllAsync(TeamSortOptions? teamSortOptions)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(t => t.IsDeleted == false);
            var accessTeams = new HashSet<Team>();
            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                accessTeams = teams.ToHashSet();
            }
            else
            {
                var loggedInUserId = AuthHelper.GetUserId()!.Value;
                var userTeams = await _userTeamDal.GetAllAsync(ut => ut.UserId == loggedInUserId);
                foreach (var userTeam in userTeams)
                {
                    var team = teams.FirstOrDefault(t => t.Id == userTeam.TeamId);
                    if (team != null) accessTeams.Add(team);
                }

                var teamManagedByUser = teams.FirstOrDefault(t => t.ManagerId == loggedInUserId);
                if (teamManagedByUser != null) accessTeams.Add(teamManagedByUser);
            }
            
            if (accessTeams.Count == 0)
            {
                result.Fail(new ErrorMessage("TM-109374", "You are not authorized to access any team"));
                return result;
            }

            var accessTeamsList = accessTeams.ToList();

            accessTeamsList = teamSortOptions switch
            {
                TeamSortOptions.Name => accessTeamsList.OrderBy(t => t.Name).ToList(),
                TeamSortOptions.MemberCount => accessTeamsList.OrderBy(t =>
                {
                    var userTeams = _userTeamDal.GetAllAsync(ut => ut.TeamId == t.Id).Result;
                    return userTeams.Count;
                    
                }).ToList(),
                TeamSortOptions.ProjectCount => accessTeamsList.OrderBy(t =>
                {
                    var teamProjects = _teamProjectDal.GetAllAsync(tp => tp.TeamId == t.Id).Result;
                    return teamProjects.Count;
                }).ToList(),
                _ => accessTeamsList
            };

            var teamDto = _mapper.Map<List<TeamGetDto>>(accessTeamsList);
            result.SetData(teamDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-109375", e.Message)); // Değişiklik: Hata kodu güncellendi
        }

        return result;
    }
    #endregion

    #region GetById
    public async Task<ServiceObjectResult<TeamGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<TeamGetDto?>();

        try
        {
            var team = await _teamDal.GetAsync(t => t.Id == id && t.IsDeleted == false);
            BusinessRules.Run(("TM-432894", BusinessRules.CheckEntityNull(team)));

            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var userTeams = await _userTeamDal.GetAllAsync(ut => ut.TeamId == id);
            
            bool hasAccess;
            
            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                hasAccess = true;
            else if(userTeams.Any(userTeam => userTeam.UserId == loggedInUserId))
                hasAccess = true;
            else
            {
                hasAccess = team.ManagerId == loggedInUserId;
            }
            
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("TM-432895", "You are not authorized to access this team"));
                return result;
            }

            var teamDto = _mapper.Map<TeamGetDto>(team);
            result.SetData(teamDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-432895", e.Message));
        }

        return result;
    }
    #endregion

    #region GetTeamByManagerId
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByManagerIdAsync(Guid managerId, TeamSortOptions? teamSortOptions)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var hasAccess = loggedInUserId == managerId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("TM-789658", "You do not have access to view this team"));
                return result;
            }

            var teams = await _teamDal.GetAllAsync(t => t.ManagerId == managerId && !t.IsDeleted);

            if (!teams.Any())
            {
                result.Fail(new ErrorMessage("TM-789658", "There is no team managed by this user"));
                return result;
            }

            // Apply sorting
            teams = teamSortOptions switch
            {
                TeamSortOptions.Name => teams.OrderBy(t => t.Name).ToList(),
                TeamSortOptions.MemberCount => teams.OrderBy(t =>
                {
                    var userTeams = _userTeamDal.GetAllAsync(ut => ut.TeamId == t.Id).Result;
                    return userTeams.Count;
                
                }).ToList(),
                TeamSortOptions.ProjectCount => teams.OrderBy(t =>
                {
                    var teamProjects = _teamProjectDal.GetAllAsync(tp => tp.TeamId == t.Id).Result;
                    return teamProjects.Count;
                }).ToList(),
                _ => teams
            };

            var teamDto = _mapper.Map<List<TeamGetDto>>(teams);
            result.SetData(teamDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-109374", e.Message));
        }

        return result;
    }
    #endregion

    #region GetTeamByProjectId
   public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByProjectIdAsync(Guid id, TeamSortOptions? teamSortOptions)
{
    var result = new ServiceCollectionResult<TeamGetDto?>();

    try
    {
        // Get the project by id
        var project = await _projectDal.GetAsync(p => p.Id == id && !p.IsDeleted);

        // Check if the project exists
        BusinessRules.Run(("PRJ-789012", BusinessRules.CheckEntityNull(project, "Project not found")));

        // Get all teams associated with the project

        // Check if the logged-in user has access to view the teams in the project
        var loggedInUserId = AuthHelper.GetUserId()!.Value;
        var hasAccess = false;

        if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            hasAccess = true;
        else
            hasAccess = project!.ManagerId == loggedInUserId;
        
        var projectTeams = await _teamProjectDal.GetAllAsync(tp => tp.ProjectId == project!.Id);
        
        // Check if the logged-in user has access to view the teams in the project
        if (!hasAccess)
        {
            result.Fail(new ErrorMessage("PRJ-789012", "You do not have access to view teams in this project"));
            return result;
        }

        // Get all teams associated with the project
        var teams = await _teamDal.GetAllAsync(t => projectTeams.Select(tp => tp.TeamId).Contains(t.Id));

        // Apply sorting
        teams = teamSortOptions switch
        {
            TeamSortOptions.Name => teams.OrderBy(t => t.Name).ToList(),
            TeamSortOptions.MemberCount => teams.OrderBy(t =>
            {
                var userTeams = _userTeamDal.GetAllAsync(ut => ut.TeamId == t.Id).Result;
                return userTeams.Count;

            }).ToList(),
            TeamSortOptions.ProjectCount => teams.OrderBy(t =>
            {
                var teamProjects = _teamProjectDal.GetAllAsync(tp => tp.TeamId == t.Id).Result;
                return teamProjects.Count;
            }).ToList(),
            _ => teams
        };

        // Check if there are any teams associated with the project
        BusinessRules.Run(("PRJ-789012",
            BusinessRules.CheckCollectionNullOrEmpty(teams, "No teams found in this project")));

        // Map the teams to DTOs
        var teamDto = _mapper.Map<List<TeamGetDto>>(teams);
        result.SetData(teamDto);
    }
    catch (ValidationException e)
    {
        result.Fail(e);
    }
    catch (Exception e)
    {
        result.Fail(new ErrorMessage("PRJ-789012", e.Message));
    }

    return result;
}
    #endregion

    #region GetTeamByName
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByNameAsync(string name, TeamSortOptions? teamSortOptions)
{
    var result = new ServiceCollectionResult<TeamGetDto?>();
    try
    {
        var loggedInUserId = AuthHelper.GetUserId()!.Value;
        
        var teams = await _teamDal.GetAllAsync(t => t.Name.Contains(name) && !t.IsDeleted);
        var accessTeams = new HashSet<Team>();
        
        if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
        {
            accessTeams = teams.ToHashSet();
        }
        else
        {
            var userTeams = await _userTeamDal.GetAllAsync(ut => ut.UserId == loggedInUserId);
            foreach (var userTeam in userTeams)
            {
                var team = teams.FirstOrDefault(t => t.Id == userTeam.TeamId);
                if (team != null) accessTeams.Add(team);
            }

            var teamManagedByUser = teams.FirstOrDefault(t => t.ManagerId == loggedInUserId);
            if (teamManagedByUser != null) accessTeams.Add(teamManagedByUser);
        }

        var accessTeamsList = accessTeams.ToList();

        // Apply sorting
        accessTeamsList = teamSortOptions switch
        {
            TeamSortOptions.Name => accessTeamsList.OrderBy(t => t.Name).ToList(),
            TeamSortOptions.MemberCount => accessTeamsList.OrderBy(t =>
            {
                var userTeams = _userTeamDal.GetAllAsync(ut => ut.TeamId == t.Id).Result;
                return userTeams.Count;

            }).ToList(),
            TeamSortOptions.ProjectCount => accessTeamsList.OrderBy(t =>
            {
                var teamProjects = _teamProjectDal.GetAllAsync(tp => tp.TeamId == t.Id).Result;
                return teamProjects.Count;
            }).ToList(),
            _ => accessTeamsList
        };

        var teamDto = _mapper.Map<List<TeamGetDto>>(accessTeamsList);
        result.SetData(teamDto);
    }
    catch (ValidationException e)
    {
        result.Fail(e);
    }
    catch (Exception e)
    {
        result.Fail(new ErrorMessage("TM-109374", e.Message));
    }

    return result;
}
    #endregion

    #region AssignUserToTeam
    public async Task<ServiceObjectResult<TeamGetDto>> AssignUsersToTeamAsync(AssignUsersToTeamDto assignUsersToTeamDto)
    {
        var result = new ServiceObjectResult<TeamGetDto>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var team = await _teamDal.GetAsync(t => t.Id == assignUsersToTeamDto.TeamId && !t.IsDeleted);
            BusinessRules.Run(("TM-109374", BusinessRules.CheckEntityNull(team, "Team not found")));
            
            bool hasAccess;
            
            if (AuthHelper.GetRole()!.Equals(UserRoles.Admin))
                hasAccess = true;
            else
            {
                // Check if the logged-in user is the manager of the team
                hasAccess = team!.ManagerId == loggedInUserId;
            }
            
            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("TM-109374", "You are not authorized to assign a user to this team"));
                return result;
            }
            
            var newUserTeams = assignUsersToTeamDto.UserIds.Select(userId => new UserTeam
            {
                TeamId = assignUsersToTeamDto.TeamId,
                UserId = userId
            });
        
            // Assign the new users
            foreach (var userTeam in newUserTeams)
            {
                // if exists, do not add
                var userTeamExists = await _userTeamDal.GetAsync(ut => ut.TeamId == userTeam.TeamId && ut.UserId == userTeam.UserId);
            
                if (userTeamExists == null) 
                    await _userTeamDal.AddAsync(userTeam);
            }
            
            result.SetData(_mapper.Map<TeamGetDto>(team));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-109374", e.Message));
        }

        return result;
    }
    #endregion

    #region Update
    public async Task<ServiceObjectResult<TeamGetDto>> UpdateAsync(TeamUpdateDto teamUpdateDto)
    {
        var result = new ServiceObjectResult<TeamGetDto>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;

            var team = await _teamDal.GetAsync(u => u.Id == teamUpdateDto.Id && u.IsDeleted == false);

            BusinessRules.Run(("TM-197302", BusinessRules.CheckEntityNull(team)));

            var hasAccess = team.ManagerId == loggedInUserId || AuthHelper.GetRole()!.Equals(UserRoles.Admin);

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("TM-197302", "You are not authorized to update this team"));
                return result;
            }
            
            team = _mapper.Map(teamUpdateDto, team); 
            await _teamDal.UpdateAsync(team);
            result.SetData(_mapper.Map<TeamGetDto>(team));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-197356", e.Message));
        }

        return result;
    }
    #endregion

    #region Create
    public async Task<ServiceObjectResult<TeamGetDto>> CreateAsync(TeamCreateDto teamCreateDto)
    {
        var result = new ServiceObjectResult<TeamGetDto>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var team = _mapper.Map<Team>(teamCreateDto);
            team.ManagerId = loggedInUserId;
            await _teamDal.AddAsync(team);
            result.SetData(_mapper.Map<TeamGetDto>(team));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-109374", e.Message));
        }

        return result;
    }
    #endregion

    #region DeleteById
    public async Task<ServiceObjectResult<TeamGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<TeamGetDto?>();

        try
        {
            var loggedInUserId = AuthHelper.GetUserId()!.Value;
            var team = await _teamDal.GetAsync(u => u.Id == id && u.IsDeleted == false);
            BusinessRules.Run(("TM-197302", BusinessRules.CheckEntityNull(team)));

            var hasAccess = (team.ManagerId == loggedInUserId || AuthHelper.GetRole()!.Equals(UserRoles.Admin));

            if (!hasAccess)
            {
                result.Fail(new ErrorMessage("TM-197302", "You are not authorized to delete this team"));
                return result;
            }
            
            await _teamDal.SoftDeleteAsync(team);
            result.SetData(_mapper.Map<TeamGetDto>(team));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-197356", e.Message));
        }

        return result;
    }
    #endregion
}