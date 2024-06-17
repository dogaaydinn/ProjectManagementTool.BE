using AutoMapper;
using Business.Constants.Messages.Services.DutyManagement;
using Business.Services.DutyManagement.Abstract;
using Core.Constants;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.Hashing;
using Core.Utils.IoC;
using Core.Utils.Rules;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.DutyManagement.UserManagement;

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

    #region ChangePassword

    public async Task<ServiceObjectResult<UserGetDto?>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            // Only allow the user themselves to reset their password
            if (AuthHelper.GetUserId() != changePasswordDto.Id && !AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                result.Fail(new ErrorMessage("USER-841618", "Unauthorized access"));
                return result;
            }

            BusinessRules.Run(
                ("USER-342385", BusinessRules.CheckDtoNull(changePasswordDto)),
                ("USER-128747",
                    changePasswordDto.NewPassword.Equals(changePasswordDto.ConfirmPassword)
                        ? null
                        : UserServiceMessages.PasswordsNotMatch));

            var user = await _userDal.GetAsync(b => changePasswordDto.Id.Equals(b.Id));
            BusinessRules.Run(("USER-500620", BusinessRules.CheckEntityNull(user)));

            var passwordCheck = HashingHelper.VerifyPasswordHash(changePasswordDto.CurrentPassword,
                user!.PasswordHash,
                user.PasswordSalt);

            BusinessRules.Run(("USER-569667", passwordCheck ? null : UserServiceMessages.IncorrectPassword));

            HashingHelper.CreatePasswordHash(changePasswordDto.NewPassword, out var passwordHash,
                out var passwordSalt);

            user!.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var userGetDto = _mapper.Map<UserGetDto>(user);
            result.SetData(userGetDto, UserServiceMessages.PasswordChanged);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USER-841618", e.Message));
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
            if (AuthHelper.GetUserId() != userUpdateDto.Id && !AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                result.Fail(new ErrorMessage("USER-452175", "Unauthorized access"));
                return result;
            }

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

    #region Delete

    public async Task<ServiceObjectResult<UserGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<UserGetDto?>();

        try
        {
            if (AuthHelper.GetUserId() != id && !AuthHelper.GetRole()!.Equals(UserRoles.Admin))
            {
                result.Fail(new ErrorMessage("USER-841618", "Unauthorized access"));
                return result;
            }

            var user = await _userDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (user == null)
            {
                result.Fail(new ErrorMessage("USMN-238975", "User not found"));
                return result;
            }

            user.IsDeleted = true;
            await _userDal.UpdateAsync(user);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("USMN-238975", e.Message));
        }

        return result;
    }

    #endregion
}