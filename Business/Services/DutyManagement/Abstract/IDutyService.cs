

using Core.Services;
using Core.Services.Result;
using Domain.DTOs.DutyManagement;

namespace Business.Services.DutyManagement.Abstract;

public interface IDutyService : IService
{
    Task<ServiceCollectionResult<DutyGetDto?>> GetAllAsync();
    Task<ServiceObjectResult<DutyGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByUserIdAsync(Guid userId);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByTitleAsync(string title);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByProjectIdAsync(Guid projectId);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByStatusAsync(int status);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByPriorityAsync(int priority);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByReporterIdAsync(Guid reporterId);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByAssigneeIdAsync(Guid assigneeId);
    Task<ServiceCollectionResult<DutyGetDto>> GetByDutyTypeAsync(int dutyType);
    Task<ServiceCollectionResult<DutyGetDto?>> GetByLabelIdAsync(Guid labelId);
    Task<ServiceObjectResult<DutyGetDto>> UpdateAsync(DutyUpdateDto dutyUpdateDto);
    Task<ServiceObjectResult<DutyGetDto>> CreateAsync(DutyCreateDto dutyCreateDto);
    Task<ServiceObjectResult<DutyGetDto?>> DeleteByIdAsync(Guid id);
}