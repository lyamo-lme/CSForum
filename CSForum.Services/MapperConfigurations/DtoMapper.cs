using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;

namespace CSForum.Services.MapperConfigurations;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<Post, CreatePost>().ReverseMap();
        CreateMap<Post, EditPost>().ReverseMap();

        CreateMap<Answer, CreateAnswer>().ReverseMap();
    }
}