using AutoMapper;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Tag, TagResponse>();
            CreateMap<Post, PostResponse>()
                .ForMember(post => post.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new TagResponse { Id = x.Id })));

        }
    }
}
