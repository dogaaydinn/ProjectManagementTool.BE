using Core.Services;
using Core.Services.Result;
using Domain.DTOs.ProjectManagement;

namespace Business.Services.ProjectManagement.Abstract;

public interface IProjectService : IService
{
    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllAsync();
    Task<ServiceObjectResult<ProjectGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetAllByManagerIdAsync(Guid id);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStatusAsync(int status);
    Task<ServiceObjectResult<ProjectGetDto>> GetProjectByPriorityAsync(int priority);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByDutyIdAsync(Guid id);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByDueDateAsync(DateTime dueDate);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStartDateAsync(DateTime startDate);
    Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByNameAsync(string name);
    Task<ServiceObjectResult<ProjectGetDto?>> UpdateAsync(ProjectUpdateDto projectUpdateDto);
    Task<ServiceObjectResult<ProjectGetDto>> CreateAsync(ProjectCreateDto projectCreateDto);
    Task<ServiceObjectResult<ProjectGetDto?>> DeleteByIdAsync(Guid id);
}