using AutoMapper;
using OrderAPI.Models.Domain;
using OrderAPI.Models.DTO.OrderDTO;

namespace OrderAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, ReducedOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
        }
    }
}
