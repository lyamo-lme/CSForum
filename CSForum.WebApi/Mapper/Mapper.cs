using AutoMapper;
using CSForum.Core.Models;
using CSForum.Shared.dtoModels;

namespace CSForum.Services.Mapper;

public class Mapper:Profile
{
    public Mapper() {
        CreateMap<Post, EditPost>().ReverseMap();
        CreateMap<Post, CreatePost>().ReverseMap();
    }
}