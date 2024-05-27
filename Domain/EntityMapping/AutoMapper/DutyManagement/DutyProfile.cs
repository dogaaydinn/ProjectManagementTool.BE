using AutoMapper;
using Domain.DTOs.DutyManagement;
using Domain.Entities.DutyManagement;

namespace Domain.EntityMapping.AutoMapper.DutyManagement;

public class DutyProfile : Profile
{
    public DutyProfile()
    {
        CreateMap<Duty, DutyGetDto>();
        CreateMap<DutyGetDto, Duty>();
        
        CreateMap<DutyCreateDto, Duty>();
        
        CreateMap<DutyUpdateDto, Duty>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) => srcMember != null));
    }
    
}