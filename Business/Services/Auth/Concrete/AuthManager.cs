using System.Text.Json;
using AutoMapper;
using Business.Constants.Messages.Services.Communication;
using Business.Services.Auth.Abstract;
using Business.Services.Communication.Abstract;
using Core.Constants;
using Core.ExceptionHandling;
using Core.Security.SessionManagement;
using Core.Services.Messages;
using Core.Services.Payload;
using Core.Services.Result;
using Core.Utils.Auth;
using Core.Utils.Hashing;
using Core.Utils.IoC;
using Core.Utils.Rules;
using DataAccess.Repositories.Abstract.UserManagement;
using Domain.DTOs.Auth;
using Domain.DTOs.DutyManagement.UserManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Business.Services.Auth.Concrete;

public class AuthManager : IAuthService
{
    private readonly IMailingService _emailService = ServiceTool.GetService<IMailingService>()!;
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly ITokenHandler _tokenHandler = ServiceTool.GetService<ITokenHandler>()!;
    private readonly IUserDal _userDal = ServiceTool.GetService<IUserDal>()!;

    #region login

    public async Task<ServiceObjectResult<LoginResponseDto?>> LoginAsync(LoginDto loginDto)
    {
        var result = new ServiceObjectResult<LoginResponseDto?>();

        try
        {
            if (loginDto.Email != null)
                BusinessRules.Run(
                    ("AUTH-366764", BusinessRules.CheckDtoNull(loginDto)),
                    ("AUTH-584337", BusinessRules.CheckEmail(loginDto.Email)));

            var user = await _userDal.GetAsync(
                u => u.Email == loginDto.Email || u.Username == loginDto.Username);

            if (user == null)
            {
                result.Fail(new ErrorMessage("AUTH-809431", AuthServiceMessages.NotFound));
                return result;
            }

            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                result.Fail(new ErrorMessage("AUTH-290694", AuthServiceMessages.WrongPassword));
                return result;
            }

            if (user.UseMultiFactorAuthentication)
            {
                user.LoginVerificationCode = GenerateMfaCode();
                user.LoginVerificationCodeExpiration = DateTime.Now.AddMinutes(1);
                user.LastLoginTime = DateTime.Now;
                await _userDal.UpdateAsync(user);
                var emailMessage =
                    $"Please use this code to login: {user.LoginVerificationCode}. The code will expire in 1 minute.";
                var mailResult = _emailService.SendSmtp(user.Email, "Verify Login", emailMessage);

                if (mailResult.HasFailed)
                {
                    var errDescription = mailResult.Messages[0].Description;
                    result.Fail(new ErrorMessage(mailResult.ResultCode!,
                        errDescription ?? AuthServiceMessages.VerificationCodeMailNotSent));

                    return result;
                }

                result.ExtraData.Add(new ServicePayloadItem("useMFA", true));
                result.Warning(AuthServiceMessages.MfaRequired);
                return result;
            }

            var token = _tokenHandler.GenerateToken(user.Id.ToString(), user.Username, user.Email, user.Role, false);

            var serializedToken = JsonSerializer.Serialize(token);
            user.ActiveToken = serializedToken;
            await _userDal.UpdateAsync(user);

            var userGetDto = _mapper.Map<UserGetDto>(user);
            result.SetData(new LoginResponseDto { Token = token!, User = userGetDto },
                AuthServiceMessages.LoginSuccessful);
        }
        catch (ValidationException ex)
        {
            result.Fail(new ErrorMessage(ex.ExceptionCode, ex.Message));
        }
        catch (Exception ex)
        {
            result.Fail(new ErrorMessage("AUTH-347466", ex.Message));
        }

        return result;
    }

    #endregion

    #region register

    public async Task<ServiceObjectResult<bool>> RegisterAsync(RegisterDto registerDto)
    {
        var result = new ServiceObjectResult<bool>();

        try
        {
            /*
            BusinessRules.Run(
                ("AUTH-310832", BusinessRules.CheckDtoNull(registerDto)),
                ("AUTH-906899", BusinessRules.CheckEmail(registerDto.Email)),
                ("AUTH-712957", await CheckIfEmailRegisteredBefore(registerDto.Email)),
                ("AUTH-770711", await CheckIfUsernameRegisteredBefore(registerDto.Username))
            );*/

            HashingHelper.CreatePasswordHash(registerDto.Password, out var passwordHash,
                out var passwordSalt);

            var user = _mapper.Map<User>(registerDto);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userDal.AddAsync(user);
            result.SetData(true, AuthServiceMessages.RegisterSuccessful);
        }
        catch (ValidationException ex)
        {
            result.Fail(new ErrorMessage(ex.ExceptionCode, ex.Message));
        }
        catch (Exception ex)
        {
            result.Fail(new ErrorMessage("AUTH-976141", ex.Message));
        }

        return result;
    }

    #endregion

    #region verify-mfa

    public async Task<ServiceObjectResult<LoginResponseDto?>> VerifyMfaCodeAsync(VerifyMfaCodeDto verifyMfaCodeDto)
    {
        var result = new ServiceObjectResult<LoginResponseDto?>();
        try
        {
            var user = await _userDal.GetAsync(p => p.Email == verifyMfaCodeDto.Email);
            if (user == null)
            {
                result.Fail(new ErrorMessage("AUTH-808079", AuthServiceMessages.NotFound));
                return result;
            }

            if (user.LoginVerificationCode != verifyMfaCodeDto.Code)
            {
                result.Fail(new ErrorMessage("AUTH-755666", AuthServiceMessages.WrongVerificationCode));
                return result;
            }

            if (user.LoginVerificationCodeExpiration < DateTime.UtcNow)
            {
                result.Fail(new ErrorMessage("AUTH-221332", AuthServiceMessages.VerificationCodeExpired));
                return result;
            }

            user.LoginVerificationCode = null;
            user.LoginVerificationCodeExpiration = null;

            await _userDal.UpdateAsync(user);

            var token = _tokenHandler.GenerateToken(user.Id.ToString(), user.Username, user.Email, user.Role, false);

            var userGetDto = _mapper.Map<UserGetDto>(user);
            result.SetData(new LoginResponseDto { Token = token!, User = userGetDto },
                AuthServiceMessages.VerificationSuccessful);
        }
        catch (ValidationException ex)
        {
            result.Fail(new ErrorMessage(ex.ExceptionCode, ex.Message));
        }
        catch (Exception ex)
        {
            result.Fail(new ErrorMessage("AUTH-562594", ex.Message));
        }

        return result;
    }

    #endregion

    #region forgot-password

    public async Task<ServiceObjectResult<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var result = new ServiceObjectResult<bool>();

        try
        {
            var user = await _userDal.GetAsync(x => x.Email == forgotPasswordDto.Email);

            if (user == null)
            {
                result.Fail(new ErrorMessage("AUTH-432894", "User not found"));
                return result;
            }

            user.ResetPasswordCode = GenerateMfaCode();
            user.ResetPasswordCodeExpiration = DateTime.Now.AddMinutes(1);
            await _userDal.UpdateAsync(user);
            var emailMessage =
                $"Please use this code to reset your password: {user.ResetPasswordCode}. The code will expire in 1 minute.";
            var mailResult = _emailService.SendSmtp(user.Email, "Reset Password", emailMessage);

            if (mailResult.HasFailed)
            {
                var errDescription = mailResult.Messages[0].Description;
                result.Fail(new ErrorMessage(mailResult.ResultCode!,
                    errDescription ?? AuthServiceMessages.VerificationCodeMailNotSent));

                return result;
            }

            result.SetData(true);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("AUTH-943056", e.Message));
        }

        return result;
    }

    #endregion

    #region logout

    public async Task<ServiceObjectResult<bool>> Logout(Guid userId)
    {
        var result = new ServiceObjectResult<bool>();
        try
        {
            var user = await _userDal.GetAsync(p => p.Id.Equals(userId));
            BusinessRules.Run(("AUTH-808079", BusinessRules.CheckEntityNull(user)));

            var loggedInUser = AuthHelper.GetUserId();
            var isAdmin = AuthHelper.GetRole()!.Equals(UserRoles.Admin);
            
            if (loggedInUser != userId && !isAdmin)
            {
                result.Fail(new ErrorMessage("AUTH-808079", AuthServiceMessages.Unauthorized));
                return result;
            }

            user!.ActiveToken = null;
            await _userDal.UpdateAsync(user);
            result.SetData(true, AuthServiceMessages.LogoutSuccessful);
        }
        catch (ValidationException ex)
        {
            result.Fail(new ErrorMessage(ex.ExceptionCode, ex.Message));
        }
        catch (Exception ex)
        {
            result.Fail(new ErrorMessage("AUTH-800477", ex.Message));
        }

        return result;
    }

    #endregion

    #region reset-password

    public async Task<ServiceObjectResult<LoginResponseDto?>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var result = new ServiceObjectResult<LoginResponseDto?>();

        try
        {
            var user = await _userDal.GetAsync(x => x.Email == resetPasswordDto.Email);

            if (user == null)
            {
                result.Fail(new ErrorMessage("AUTH-432894", "User not found"));
                return result;
            }

            if (user.ResetPasswordCode != resetPasswordDto.LoginVerificationCode)
            {
                result.Fail(new ErrorMessage("AUTH-755666", AuthServiceMessages.WrongVerificationCode));
                return result;
            }

            if (user.ResetPasswordCodeExpiration == null || user.ResetPasswordCodeExpiration < DateTime.UtcNow)
            {
                result.Fail(new ErrorMessage("AUTH-221332", AuthServiceMessages.VerificationCodeExpired));
                return result;
            }

            HashingHelper.CreatePasswordHash(resetPasswordDto.NewPassword, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordCode = null;
            user.ResetPasswordCodeExpiration = null;

            await _userDal.UpdateAsync(user);

            var token = _tokenHandler.GenerateToken(user.Id.ToString(), user.Username, user.Email, user.Role, false);

            if (token == null)
            {
                result.Fail(new ErrorMessage("AUTH-564321", "Failed to generate token"));
                return result;
            }

            var userGetDto = _mapper.Map<UserGetDto>(user);
            result.SetData(new LoginResponseDto { Token = token, User = userGetDto },
                AuthServiceMessages.PasswordResetSuccessful);
        }
        catch (ValidationException ex)
        {
            result.Fail(new ErrorMessage(ex.ExceptionCode, ex.Message));
        }
        catch (Exception ex)
        {
            result.Fail(new ErrorMessage("AUTH-562594", "An unexpected error occurred: " + ex.Message));
        }

        return result;
    }

    #endregion

    private static string GenerateMfaCode()
    {
        var code = new Random().Next(100000, 999999).ToString();
        return code;
    }

    private async Task<string?> CheckIfUsernameRegisteredBefore(string username)
    {
        var user = await _userDal.GetAsync(p => p.Username == username);
        return user != null ? AuthServiceMessages.UsernameAlreadyRegistered : null;
    }

    private async Task<string?> CheckIfEmailRegisteredBefore(string email)
    {
        var user = await _userDal.GetAsync(p => p.Email == email);
        return user != null ? AuthServiceMessages.EmailAlreadyRegistered : null;
    }
}