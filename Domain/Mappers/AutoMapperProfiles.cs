using AutoMapper;
using Domain.Entities;

namespace Domain.Mappers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductMovement, ProductMovementDto>().ReverseMap();
        CreateMap<Depot, DepotDto>().ReverseMap();
    }
}
