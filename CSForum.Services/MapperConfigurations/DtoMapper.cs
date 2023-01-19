using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.dtoModels;

namespace CSForum.Services.MapperConfigurations;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<Post, CreatePost>().ReverseMap();
    }
}