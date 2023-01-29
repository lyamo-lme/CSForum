using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tag;
using CSForum.WebUI.Models;

namespace CSForum.Services.MapperConfigurations;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<Post, EditPostDto>().ReverseMap();

        CreateMap<Answer, CreateAnswerDto>().ReverseMap();
        CreateMap<CreateAnswerViewModel,CreateAnswerDto>().ReverseMap();
        
        CreateMap<Tag, CreateTagDto>().ReverseMap();
        CreateMap<Tag, EditTagDto>().ReverseMap();
    }
}