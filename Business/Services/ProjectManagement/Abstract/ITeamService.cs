using Core.Constants.SortOptions;
using Core.Services;
using Core.Services.Result;
using Domain.DTOs.ProjectManagement;

namespace Business.Services.ProjectManagement.Abstract;

public interface ITeamService : IService
{
    Task<ServiceCollectionResult<TeamGetDto?>> GetAllAsync(TeamSortOptions? teamSortOptions);
    Task<ServiceObjectResult<TeamGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByManagerIdAsync(Guid id, TeamSortOptions? teamSortOptions);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByProjectIdAsync(Guid id, TeamSortOptions? teamSortOptions);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByNameAsync(string name, TeamSortOptions? teamSortOptions);
    Task<ServiceObjectResult<TeamGetDto>> AssignUsersToTeamAsync(AssignUsersToTeamDto assignUsersToTeamDto);
    Task<ServiceObjectResult<TeamGetDto>> UpdateAsync(TeamUpdateDto teamUpdateDto);
    Task<ServiceObjectResult<TeamGetDto>> CreateAsync(TeamCreateDto teamCreateDto);
    Task<ServiceObjectResult<TeamGetDto?>> DeleteByIdAsync(Guid id);
}