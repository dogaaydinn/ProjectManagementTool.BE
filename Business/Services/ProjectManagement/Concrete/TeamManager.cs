using AutoMapper;
using Business.Services.ProjectManagement.Abstract;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.ProjectManagement;
using Domain.DTOs.ProjectManagement;
using Core.ExceptionHandling;
using Domain.Entities.ProjectManagement;

namespace Business.Services.ProjectManagement.Concrete;

public class TeamManager : ITeamService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly ITeamDal _teamDal = ServiceTool.GetService<ITeamDal>()!;

    #region GetAllAsync
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.IsDeleted == false);
            var teamDto = _mapper.Map<List<TeamGetDto>>(teams);
            result.SetData(teamDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-789658", e.Message));
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
            var team = await _teamDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            var teamDto = _mapper.Map<TeamGetDto>(team);
            result.SetData(teamDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("TM-432894", e.Message));
        }
        
        return result;

    }


    #endregion

    #region GetTeamByManagerId
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByManagerIdAsync(Guid id)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.ManagerId == id && x.IsDeleted == false);
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
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByProjectIdAsync(Guid id)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.ProjectId == id && x.IsDeleted == false);
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

    #region GetTeamByName
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByNameAsync(string name)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.Name == name && x.IsDeleted == false);
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

    #region GetTeamByStatus
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByStatusAsync(int status)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.Status == status && x.IsDeleted == false);
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

    #region GetTeamByPriority
    public async Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByPriorityAsync(int priority)
    {
        var result = new ServiceCollectionResult<TeamGetDto?>();
        try
        {
            var teams = await _teamDal.GetAllAsync(x => x.Priority == priority && x.IsDeleted == false);
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

    #region Update
    public async Task<ServiceObjectResult<TeamGetDto>> UpdateAsync(TeamUpdateDto teamUpdateDto)
    {
        var result = new ServiceObjectResult<TeamGetDto>();

        try
        {
            var team = await _teamDal.GetAsync(u => u.Id == teamUpdateDto.Id && u.IsDeleted == false);

            if (team == null)
            {
                result.Fail(new ErrorMessage("TM-197302", "Team not found"));
                return result;
            }

            _mapper.Map(teamUpdateDto, team);
            
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
            var team = _mapper.Map<Team>(teamCreateDto);
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
            var team = await _teamDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (team == null)
            {
                result.Fail(new ErrorMessage("TM-197302", "Team not found"));
                return result;
            }

            await _teamDal.SoftDeleteAsync(team);
            var teamDto = _mapper.Map<TeamGetDto>(team);
            result.SetData(teamDto);
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