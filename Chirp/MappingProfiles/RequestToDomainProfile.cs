using AutoMapper;
using Chirp.Contracts;
using Chirp.Contracts.V1.Requests.Queries;
using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllPostsFilterQuery, GetAllPostsFilter>();
        }
    }
}
