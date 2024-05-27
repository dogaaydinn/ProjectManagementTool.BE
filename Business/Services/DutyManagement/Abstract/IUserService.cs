using Core.Services;
using Core.Services.Result;
using Domain.DTOs.DutyManagement.UserManagement;

namespace Business.Services.DutyManagement.Abstract;

public interface IUserService : IService
{
    Task<ServiceCollectionResult<UserGetDto?>> GetAllAsync();
    Task<ServiceObjectResult<UserGetDto?>> GetByIdAsync(Guid id);
    Task<ServiceCollectionResult<UserGetDto?>> GetByRoleAsync(string role);
    Task<ServiceObjectResult<UserGetDto?>> GetByUsernameAsync(string username);
    Task<ServiceObjectResult<UserGetDto?>> GetByEmailAsync(string email);
    Task<ServiceObjectResult<UserGetDto?>> GetByPhoneNumberAsync(string phoneNumber);
    Task<ServiceObjectResult<UserGetDto?>> UpdateAsync(UserUpdateDto projectUpdateDto);
    Task<ServiceObjectResult<UserGetDto>> CreateAsync(UserCreateDto userCreateDto);
    Task<ServiceObjectResult<UserGetDto?>> DeleteByIdAsync(Guid id);
}