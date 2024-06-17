using AutoMapper;
using Domain.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Domain.EntityMapping.AutoMapper.ProjectManagement;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamGetDto>();
        CreateMap<TeamGetDto, Team>();

        CreateMap<TeamCreateDto, Team>();
        CreateMap<TeamUpdateDto, Team>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (opts.DestinationMember.Name == "ManagerId" && srcMember is Guid guid) return guid != Guid.Empty;

                return srcMember != null;
            }));
    }
}