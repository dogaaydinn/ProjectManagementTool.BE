using AutoMapper;
using Domain.DTOs.Auth;
using Domain.DTOs.DutyManagement.UserManagement;
using Domain.Entities.DutyManagement.UserManagement;

namespace Domain.EntityMapping.AutoMapper.DutyManagement.UserManagement;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserGetDto>();
        CreateMap<UserGetDto, User>();

        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserCreateDto>();

        CreateMap<RegisterDto, User>();

        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (opts.DestinationMember.Name == "RoleId" && srcMember is Guid guid) return guid != Guid.Empty;

                return srcMember != null;
            }));
    }
}