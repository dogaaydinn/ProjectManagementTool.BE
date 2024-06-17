using AutoMapper;
using Domain.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Domain.EntityMapping.AutoMapper.ProjectManagement;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectGetDto>();
        CreateMap<ProjectGetDto, Project>();

        // soldakini sağdakine dönüştür
        CreateMap<ProjectCreateDto, Project>();

        CreateMap<ProjectUpdateDto, Project>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (opts.DestinationMember.Name == "ManagerId" && srcMember is Guid guid) return guid != Guid.Empty;

                return srcMember != null;
            }));
    }
}