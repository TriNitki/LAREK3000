using AutoMapper;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;
using DeliveryAPI.Repositories.IRepositories;
using DeliveryAPI.Service.IService;

namespace DeliveryAPI.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly ICourierReceiptRepository courierReceiptRepository;
        private readonly IPickupReceiptRepository pickupReceiptRepository;
        private readonly IMapper mapper;

        public ReceiptService(ICourierReceiptRepository courierReceiptRepository, IPickupReceiptRepository pickupReceiptRepository, IMapper mapper)
        {
            this.courierReceiptRepository = courierReceiptRepository;
            this.pickupReceiptRepository = pickupReceiptRepository;
            this.mapper = mapper;
        }

        public async Task<CourierReceiptDto?> Create(CreateCourierReceiptDto courierReceiptDto)
        {
            var receiptModel = mapper.Map<CourierReceipt>(courierReceiptDto);
            var receiptDomain = await courierReceiptRepository.CreateAsync(receiptModel);

            if (receiptDomain == null)
            {
                return null;
            }

            var receiptDto = mapper.Map<CourierReceiptDto>(receiptDomain);

            return receiptDto;
        }

        public async Task<PickupReceiptDto?> Create(CreatePickupReceiptDto pickupReceiptDto)
        {
            var receiptModel = mapper.Map<PickupReceipt>(pickupReceiptDto);
            var receiptDomain = await pickupReceiptRepository.CreateAsync(receiptModel);

            if (receiptDomain == null)
            {
                return null;
            }

            var receiptDto = mapper.Map<PickupReceiptDto>(receiptDomain);

            return receiptDto;
        }
    }
}
