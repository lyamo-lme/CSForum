using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Posts;
using CSForum.Shared.Models.dtoModels.Tags;
using CSForum.Shared.Models.ViewModels;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Identity;

namespace CSForum.Infrastructure.MapperConfigurations;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        //users
        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<User, IdentityUser<int>>().ReverseMap();
        
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
        CreateMap<Tag, TagIdInPostDto>().ReverseMap();
        CreateMap<Tag, TagViewModel>().ReverseMap();
       
        //postTags
        CreateMap<PostTag, TagIdInPostDto>().ReverseMap();
        CreateMap<PostTag, PostTagsViewModel>().ReverseMap();
    }
}