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

        CreateMap<CommentUpdateDto, Comment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
            {
                return opts.DestinationMember.Name switch
                {
                    "DutyId" when srcMember is Guid guid => guid != Guid.Empty,
                    "ReplyToId" when srcMember is Guid guid => guid != Guid.Empty,
                    _ => srcMember != null
                };
            }));
    }
}