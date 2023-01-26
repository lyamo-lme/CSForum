using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tag;

namespace CSForum.Services.MapperConfigurations;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<Post, EditPostDto>().ReverseMap();

        CreateMap<Answer, CreateAnswer>().ReverseMap();

        CreateMap<Tag, CreateTagDto>().ReverseMap();
        CreateMap<Tag, EditTagDto>().ReverseMap();
    }
}