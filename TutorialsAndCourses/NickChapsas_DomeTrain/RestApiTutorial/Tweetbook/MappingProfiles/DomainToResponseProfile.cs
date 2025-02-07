﻿using AutoMapper;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;

namespace Tweetbook.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.Tags,
                option => option.MapFrom(
                    src => src.Tags.Select(x => new TagResponse { Name = x.TagName })));
        CreateMap<Tag, TagResponse>();
        CreateMap<Post, PostCreatedResponse>();
    }
}