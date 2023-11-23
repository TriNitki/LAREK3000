using AutoMapper;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Models.Enum;
using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;
using DeliveryAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly ICourierReceiptRepository courierReceiptRepository;
        private readonly IPickupReceiptRepository pickupReceiptRepository;
        private readonly IDeliveryRepository deliveryRepository;
        private readonly IMapper mapper;

        public ReceiptController(ICourierReceiptRepository courierReceiptRepository, IPickupReceiptRepository pickupReceiptRepository, IDeliveryRepository deliveryRepository, IMapper mapper)
        {
            this.courierReceiptRepository = courierReceiptRepository;
            this.pickupReceiptRepository = pickupReceiptRepository;
            this.deliveryRepository = deliveryRepository;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpGet("{deliveryId:Guid}")]
        public async Task<IActionResult> GetByDeliveryId([FromRoute] Guid deliveryId)
        {
            var deliveryDomain = await deliveryRepository.GetByIdAsync(deliveryId);
            if (deliveryDomain == null)
            {
                return NotFound();
            }

            switch (deliveryDomain.ReceiptMethod)
            {
                case ReceiptMethodEnum.Pickup:
                {
                    var courierReceiptDomain = await courierReceiptRepository.GetByDeliveryIdAsync(deliveryId);
                    if (courierReceiptDomain == null)
                    {
                        return NotFound();
                    }

                    var courierReceiptDto = mapper.Map<CourierReceiptDto>(courierReceiptDomain);

                    return Ok(courierReceiptDto);
                }
                case ReceiptMethodEnum.Courier:
                {
                    var pickupReceiptDomain = await pickupReceiptRepository.GetByDeliveryIdAsync(deliveryId);
                    if (pickupReceiptDomain == null)
                    {
                        return NotFound();
                    }

                    var pickupReceiptDto = mapper.Map<PickupReceiptDto>(pickupReceiptDomain);

                    return Ok(pickupReceiptDto);
                }
                default: return BadRequest();
            }
            
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCourierReceipt([FromBody] CreateCourierReceiptDto courierReceiptDto)
        {
            var receiptModel = mapper.Map<CourierReceipt>(courierReceiptDto);
            var receiptDomain = await courierReceiptRepository.CreateAsync(receiptModel);

            if (receiptDomain == null)
            {
                return BadRequest();
            }

            var receiptDto = mapper.Map<CourierReceiptDto>(receiptDomain);

            return Ok(receiptDto);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePickupReceipt([FromBody] CreatePickupReceiptDto pickupReceiptDto)
        {
            var receiptModel = mapper.Map<PickupReceipt>(pickupReceiptDto);
            var receiptDomain = await pickupReceiptRepository.CreateAsync(receiptModel);

            if (receiptDomain == null)
            {
                return BadRequest();
            }

            var receiptDto = mapper.Map<PickupReceiptDto>(receiptDomain);

            return Ok(receiptDto);
        }
    }
}
