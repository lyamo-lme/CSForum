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
        //users
        CreateMap<User, UserViewModel>().ReverseMap();
        
        //posts
        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<Post, EditPostDto>().ReverseMap();
        CreateMap<Post, PostViewModel>().ReverseMap();
        CreateMap<CreatePostView, CreatePostDto>().ReverseMap();

        //answers
        CreateMap<Answer, CreateAnswerDto>().ReverseMap();
        CreateMap<Answer, AnswerViewModel>().ReverseMap();
        CreateMap<CreateAnswerViewModel,CreateAnswerDto>().ReverseMap();
        
        //tags
        CreateMap<Tag, CreateTagDto>().ReverseMap();
        CreateMap<Tag, EditTagDto>().ReverseMap();
    }
}