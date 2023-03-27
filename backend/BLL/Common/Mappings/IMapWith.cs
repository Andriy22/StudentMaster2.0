using AutoMapper;

namespace backend.BLL.Common.Mappings;

public interface IMapWith<T>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}