using AutoMapper;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Domain;

namespace DragonflyTracker.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsQuery, GetAllPostsFilter>();
        }
    }
}