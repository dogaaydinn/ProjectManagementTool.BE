using AutoMapper;
using Domain.DTOs.Communication;
using Domain.Entities.Communication;

namespace Domain.EntityMapping.AutoMapper.Communication;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentGetDto>();
        CreateMap<CommentGetDto, Comment>();

        CreateMap<CommentCreateDto, Comment>();
        CreateMap<Comment, CommentCreateDto>();
        CreateMap<Comment, CommentGetDto>()
            .ForMember(dest => dest.SubComments, opt => opt.MapFrom(src => src.SubComments));
        CreateMap<CommentUpdateDto, Comment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (opts.DestinationMember.Name == "DutyId" || opts.DestinationMember.Name == "ReplyToId")
                    if (srcMember is Guid guid)
                        return guid != Guid.Empty;
                return srcMember != null;
            }));
    }
}