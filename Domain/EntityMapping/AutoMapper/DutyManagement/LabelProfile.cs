using AutoMapper;
using Domain.DTOs.DutyManagement;
using Domain.Entities.DutyManagement;

namespace Domain.EntityMapping.AutoMapper.DutyManagement;

public class LabelProfile : Profile
{
    public LabelProfile()
    {
         CreateMap<Label, LabelGetDto>();
         CreateMap<LabelGetDto, Label>();
         
         CreateMap<LabelUpdateDto, Label>();
         CreateMap<Label, LabelGetDto>();
         
         CreateMap<LabelCreateDto, Label>();
         
         CreateMap<LabelUpdateDto, Label>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
             {
                 if (opts.DestinationMember.Name == "id" && srcMember is Guid guid)
                 {
                     return guid != Guid.Empty;
                 }
                
                 return srcMember != null;
             }));
    }
}