using AutoMapper;
using Business.Services.DutyManagement.Abstract;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Hashing;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.DutyManagement.UserManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Business.Services.DutyManagement.Concrete;

public class UserManager : IUserService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IUserDal _userDal = ServiceTool.GetService<IUserDal>()!;

    #region GetAll
    public async Task<ServiceCollectionResult<UserGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<UserGetDto?>();

        try
        {
            var users = await _userDal.GetAllAsync(x => x.IsDeleted == false);
            var userDtos = _mapper.Map<List<UserGetDto>>(users);
            result.SetData(userDtos); 
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-789658", e.Message));
        }

        return result;
    }
    #endregion

    #region GetById
    public async Task<ServiceObjectResult<UserGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.Id == id && u.IsDeleted == false);
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-567953", e.Message));
        }

        return result;
        
    }
    #endregion

    #region GetByRole
    public async Task<ServiceCollectionResult<UserGetDto?>> GetByRoleAsync(string role)
    {
        var result = new ServiceCollectionResult<UserGetDto?>();

        try
        {
            var users = await _userDal.GetAllAsync(u => u.Role == role && u.IsDeleted == false);
            var userDtos = _mapper.Map<List<UserGetDto>>(users);
            result.SetData(userDtos);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238457", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByUsername
    public async Task<ServiceObjectResult<UserGetDto?>> GetByUsernameAsync(string username)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.Username == username && u.IsDeleted == false);
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238457", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByEmail
    public async Task<ServiceObjectResult<UserGetDto?>> GetByEmailAsync(string email)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.Email == email && u.IsDeleted == false);
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238457", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByPhoneNumber
    public async Task<ServiceObjectResult<UserGetDto?>> GetByPhoneNumberAsync(string phone)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.PhoneNumber == phone && u.IsDeleted == false);
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238457", e.Message));
        }

        return result;
    }
    #endregion

    #region Update
    public async Task<ServiceObjectResult<UserGetDto?>> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.Id == userUpdateDto.Id && u.IsDeleted == false);

            if (user == null)
            {
                result.Fail(new ErrorMessage("USMN-238754", "User not found"));
                return result;
            }

            var updatedUser = _mapper.Map(userUpdateDto, user);
            await _userDal.UpdateAsync(updatedUser);
            var userDto = _mapper.Map<UserGetDto>(updatedUser);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238754", e.Message));
        }

        return result;
    }
    #endregion

    #region Create
    public async Task<ServiceObjectResult<UserGetDto>> CreateAsync(UserCreateDto userCreateDto)
    {
        var result = new ServiceObjectResult<UserGetDto>();

        try{
            byte[] passwordHash, passwordSalt;
        
            HashingHelper.CreatePasswordHash(userCreateDto.Password, out passwordHash, out passwordSalt);
            var user = _mapper.Map<User>(userCreateDto);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _userDal.AddAsync(user); // kullanıcı oluştu
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-456789", e.Message));
        }

        return result;
    }
    #endregion

    #region DeleteById
    public async Task<ServiceObjectResult<UserGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            var user = await _userDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (user == null)
            {
                result.Fail(new ErrorMessage("USMN-238754", "User not found"));
                return result;
            }

            await _userDal.SoftDeleteAsync(user);
            var userDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-972394", e.Message));
        }

        return result;
    }
    #endregion
}
