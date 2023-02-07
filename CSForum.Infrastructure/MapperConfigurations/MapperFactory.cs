using AutoMapper;

namespace CSForum.Services.MapperConfigurations;

public static class MapperFactory
{
    public static Mapper CreateMapper<T>() where T : Profile, new()
    {
        return new Mapper(new MapperConfiguration(conf =>
            conf.AddProfile(new T())));
    }
}