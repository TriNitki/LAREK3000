using AutoMapper;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.DeliveryDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;

namespace DeliveryAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Delivery, DeliveryDto<CourierReceiptDto>>().ReverseMap();
            CreateMap<Delivery, DeliveryDto<PickupReceiptDto>>().ReverseMap();
            CreateMap<Delivery, CreateDeliveryDto>().ReverseMap();
            CreateMap<Delivery, ReducedDeliveryDto>().ReverseMap();

            CreateMap<CourierReceipt, CreateCourierReceiptDto>().ReverseMap();
            CreateMap<CourierReceipt, CourierReceiptDto>().ReverseMap();
            CreateMap<CourierReceipt, ReducedCourierReceiptDto>().ReverseMap();

            CreateMap<PickupReceipt, CreatePickupReceiptDto>().ReverseMap();
            CreateMap<PickupReceipt, PickupReceiptDto>().ReverseMap();
        }
    }
}
