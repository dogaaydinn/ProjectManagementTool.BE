using Core.Constants;
using Core.Services;
using Core.Services.Result;
using Domain.DTOs.ProjectManagement;

namespace Business.Services.ProjectManagement.Abstract;

public interface ITeamService : IService
{
    Task<ServiceCollectionResult<TeamGetDto?>> GetAllAsync(TeamSortOptions? teamSortOptions);
    Task<ServiceObjectResult<TeamGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByManagerIdAsync(Guid id);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByProjectIdAsync(Guid id);
    Task<ServiceCollectionResult<TeamGetDto?>> GetTeamByNameAsync(string name);
    Task<ServiceObjectResult<TeamGetDto>> UpdateAsync(TeamUpdateDto teamUpdateDto);
    Task<ServiceObjectResult<TeamGetDto>> CreateAsync(TeamCreateDto teamCreateDto);
    Task<ServiceObjectResult<TeamGetDto?>> DeleteByIdAsync(Guid id);
}