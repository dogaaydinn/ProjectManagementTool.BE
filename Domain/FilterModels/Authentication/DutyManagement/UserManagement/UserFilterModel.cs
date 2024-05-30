using Core.Services;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.FilterModels.Authentication.DutyManagement.UserManagement;

public class UserFilterModel : IServiceFilterModel<User>
{
    public string? NameQuery { get; set; } = null;
    public bool? EmailVerified { get; set; } = null;
    public bool? PhoneNumberVerified { get; set; } = null;
}