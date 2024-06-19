using Core.Constants.SortOptions;
using Core.Services;
using Core.Services.Result;
using Domain.DTOs.ProjectManagement;

namespace Business.Services.ProjectManagement.Abstract;

public interface IProjectService : IService
{
    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllAsync(ProjectSortOptions? projectSortOptions);
    Task<ServiceObjectResult<ProjectGetDto?>> GetByIdAsync(Guid id);

    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllByManagerIdAsync(Guid id,
        ProjectSortOptions? projectSortOptions);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStatusAsync(int status,ProjectSortOptions? projectSortOptions);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByPriorityAsync(int priority,ProjectSortOptions? projectSortOptions);
    Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByDutyIdAsync(Guid dutyId);
    Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByNameAsync(string name);
    Task<ServiceObjectResult<ProjectGetDto?>> AssignTeamToProject(AssignTeamToProjectDto assignTeamToProjectDto);
    Task<ServiceObjectResult<ProjectGetDto?>> ChangeManagerOfExistingProject(
        ChangeManagerOfExistingProjectDto changeManagerOfExistingProjectDto);
    Task<ServiceObjectResult<ProjectGetDto?>> UpdateAsync(ProjectUpdateDto projectUpdateDto);
    Task<ServiceObjectResult<ProjectGetDto>> CreateAsync(ProjectCreateDto projectCreateDto);
    Task<ServiceObjectResult<ProjectGetDto?>> DeleteByIdAsync(Guid id);
}