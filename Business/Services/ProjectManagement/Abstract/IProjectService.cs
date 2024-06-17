using Core.Constants;
using Core.Services;
using Core.Services.Result;
using Domain.DTOs.ProjectManagement;

namespace Business.Services.ProjectManagement.Abstract;

public interface IProjectService : IService
{
    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllAsync(ProjectSortOptions? projectSortOptions);
    Task<ServiceObjectResult<ProjectGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllByManagerIdAsync(Guid id);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStatusAsync(int status);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByPriorityAsync(int priority);
    Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByDutyIdAsync(Guid dutyId);
    Task<ServiceObjectResult<ProjectGetDto?>> GetProjectByNameAsync(string name);
    Task<ServiceObjectResult<ProjectGetDto?>> ChangeManagerOfExistingProject(Guid projectId, Guid newManagerId);
    Task<ServiceObjectResult<ProjectGetDto?>> UpdateAsync(ProjectUpdateDto projectUpdateDto);
    Task<ServiceObjectResult<ProjectGetDto>> CreateAsync(ProjectCreateDto projectCreateDto);
    Task<ServiceObjectResult<ProjectGetDto?>> DeleteByIdAsync(Guid id);
}