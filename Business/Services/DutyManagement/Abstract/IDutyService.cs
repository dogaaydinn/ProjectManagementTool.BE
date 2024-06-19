using Core.Constants;
using Core.Constants.Duty;
using Core.Constants.SortOptions;
using Core.Services;
using Core.Services.Result;
using Domain.DTOs.DutyManagement;

namespace Business.Services.DutyManagement.Abstract;

public interface IDutyService : IService
{
    Task<ServiceCollectionResult<DutyGetDto?>> GetAllAsync(DutySortOptions? dutySortOptions);
    Task<ServiceObjectResult<DutyGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByUserIdAsync(Guid userId,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByTitleAsync(string title,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByProjectIdAsync(Guid projectId,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByStatusAsync(DutyStatus status,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByPriorityAsync(Priority priority,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByReporterIdAsync(Guid reporterId,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByAssigneeIdAsync(Guid assigneeId,DutySortOptions? dutySortOptions);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByDutyTypeAsync(DutyType dutyType,DutySortOptions? dutySortOptions);
    Task<ServiceObjectResult<bool>> RemoveDutyFromProjectAsync(RemoveDutyFromProjectDto removeDutyFromProjectDto);
    Task<ServiceObjectResult<DutyGetDto>> UpdateAsync(DutyUpdateDto dutyUpdateDto);
    Task<ServiceObjectResult<DutyGetDto>> CreateAsync(DutyCreateDto dutyCreateDto);
    Task<ServiceObjectResult<DutyGetDto?>> DeleteByIdAsync(Guid id);
}