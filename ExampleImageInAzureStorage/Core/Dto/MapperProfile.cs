using AutoMapper;
using ExampleImageInAzureStorage.Core.Entities;
using NetTopologySuite.Geometries;

namespace ExampleImageInAzureStorage.Core.Dto
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ExampleStorage, ExampleStorageDto>().ReverseMap();
            CreateMap<ExampleStorage, ExampleStorageDto>().ForMember(x => x.Picture, options => options.Ignore());
        }
    }
}
