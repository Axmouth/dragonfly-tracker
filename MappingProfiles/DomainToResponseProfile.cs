using System.Linq;
using AutoMapper;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;

namespace DragonflyTracker.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt => 
                    opt.MapFrom(src => src.Tags.Select(x => new TagResponse{Name = x.TagName})));
            
            CreateMap<Tag, TagResponse>();
        }
    }
}