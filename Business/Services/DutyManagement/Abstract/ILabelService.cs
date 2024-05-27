using Core.Services;
using Core.Services.Result;
using Domain.DTOs.DutyManagement;

namespace Business.Services.DutyManagement.Abstract;

public interface ILabelService : IService
{
    Task<ServiceCollectionResult<LabelGetDto?>> GetAllAsync();
    Task<ServiceObjectResult<LabelGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<LabelGetDto?>> GetByColorAsync(string color);
    Task<ServiceObjectResult<LabelGetDto?>> UpdateAsync(LabelUpdateDto labelUpdateDto);
    Task<ServiceObjectResult<LabelGetDto>> CreateAsync(LabelCreateDto labelCreateDto);
    Task<ServiceObjectResult<LabelGetDto?>> DeleteByIdAsync(Guid id);
    
}