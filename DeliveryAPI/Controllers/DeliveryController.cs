using AutoMapper;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.DeliveryDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;
using DeliveryAPI.Models.Enum;
using DeliveryAPI.Repositories.IRepositories;
using DeliveryAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DeliveryAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryRepository deliveryRepository;
        private readonly ICourierReceiptRepository courierReceiptRepository;
        private readonly IPickupReceiptRepository pickupReceiptRepository;
        private readonly IDeliveryTimeService deliveryTimeService;
        private readonly IOrderService orderService;
        private readonly IAuthService authService;
        private readonly IReceiptService receiptService;
        private readonly ICourierService courierService;
        private readonly IMapper mapper;

        public DeliveryController(
            IDeliveryRepository deliveryRepository, ICourierReceiptRepository courierReceiptRepository, 
            IPickupReceiptRepository pickupReceiptRepository, IDeliveryTimeService deliveryTimeService, 
            IOrderService orderService, IAuthService authService, IReceiptService receiptService, 
            ICourierService courierService, IMapper mapper)
        {
            this.deliveryRepository = deliveryRepository;
            this.courierReceiptRepository = courierReceiptRepository;
            this.pickupReceiptRepository = pickupReceiptRepository;
            this.deliveryTimeService = deliveryTimeService;
            this.orderService = orderService;
            this.authService = authService;
            this.receiptService = receiptService;
            this.courierService = courierService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryDto createDeliveryDto)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (accessToken.ToString() == null)
            {
                return Unauthorized("Incorrect access token");
            }

            var deliveryModel = mapper.Map<Delivery>(createDeliveryDto);
            deliveryModel.ShippingDT = await deliveryTimeService.CalculateShippingDT(createDeliveryDto.OrderId, createDeliveryDto.Address);

            var responseOrder = await orderService.GetOrderByIdAsync(accessToken.ToString(), createDeliveryDto.OrderId);

            if (responseOrder == null || !responseOrder.IsSuccess || responseOrder.Result == null)
            {
                return NotFound("Order was not found");
            }

            var deliveryDomain = await deliveryRepository.CreateAsync(deliveryModel);

            if (deliveryDomain == null)
            {
                return BadRequest("Delivery wasn't created");
            }

            switch (deliveryDomain.ReceiptMethod)
            {
                case (ReceiptMethodEnum.Courier):
                    {
                        var courierDto = await courierService.FindCourier(deliveryDomain.OrderId, accessToken);
                        if (courierDto == null)
                        {
                            return NotFound("Courier wasn't found");
                        }

                        var courierProfit = await courierService.CalculateProfit(courierDto.Id, deliveryDomain.OrderId, createDeliveryDto.Address);

                        var deliveryDt = await deliveryTimeService.CalculateCourierDeliveryDT(deliveryModel.ShippingDT, createDeliveryDto.Address);

                        var createReceiptDto = new CreateCourierReceiptDto()
                        {
                            DeliveryId = deliveryDomain.Id,
                            CourierId = courierDto.Id,
                            CourierProfit = courierProfit,
                            DeliveryDT = deliveryDt,
                            DeliveryAddress = createDeliveryDto.Address
                        };

                        var receiptDto = await receiptService.Create(createReceiptDto);
                        if (receiptDto == null)
                        {
                            return BadRequest("Receipt wasn't created");
                        }

                        receiptDto.Courier = courierDto;

                        var deliveryDto = mapper.Map<DeliveryDto<CourierReceiptDto>>(deliveryDomain);
                        deliveryDto.Order = responseOrder.Result;
                        deliveryDto.ReceiptInfo = receiptDto;

                        return Ok(deliveryDto);
                    }
                case (ReceiptMethodEnum.Pickup):
                    {
                        var availableGapDto = await deliveryTimeService.CalculateAvailableGap(createDeliveryDto.OrderId, createDeliveryDto.Address);

                        var createReceiptDto = new CreatePickupReceiptDto()
                        {
                            DeliveryId = deliveryDomain.Id,
                            AvailableFromDT = availableGapDto.AvailableFromDT,
                            AvailableToDT = availableGapDto.AvailableToDT
                        };

                        var receiptDto = await receiptService.Create(createReceiptDto);
                        if (receiptDto == null)
                        {
                            return BadRequest("Receipt wasn't created");
                        }

                        var deliveryDto = mapper.Map<DeliveryDto<PickupReceiptDto>>(deliveryDomain);
                        deliveryDto.Order = responseOrder.Result;
                        deliveryDto.ReceiptInfo = receiptDto;

                        return Ok(deliveryDto);
                    }
                default: return BadRequest("Wrong Receipt method have been passed") ;
            }
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (accessToken.ToString() == null)
            {
                return Unauthorized();
            }

            var deliveryDomain = await deliveryRepository.GetByIdAsync(id);
            if (deliveryDomain == null)
            {
                return NotFound();
            }

            var orderResponse = await orderService.GetOrderByIdAsync(accessToken, deliveryDomain.OrderId);
            if (orderResponse == null || !orderResponse.IsSuccess || orderResponse.Result == null)
            {
                return NotFound();
            }
            var orderDto = orderResponse.Result;

            switch (deliveryDomain.ReceiptMethod)
            {
                case (ReceiptMethodEnum.Courier):
                    {
                        var receiptDomain = await courierReceiptRepository.GetByDeliveryIdAsync(id);
                        if (receiptDomain == null)
                        {
                            return NotFound();
                        }

                        var courierResponse = await authService.GetUserByIdAsync(receiptDomain.CourierId);
                        if (courierResponse == null || !courierResponse.IsSuccess || courierResponse.Result == null)
                        {
                            return BadRequest();
                        }

                        var receiptDto = mapper.Map<CourierReceiptDto>(receiptDomain);
                        receiptDto.Courier = courierResponse.Result;

                        var deliveryDto = mapper.Map<DeliveryDto<CourierReceiptDto>>(deliveryDomain);
                        deliveryDto.Order = orderDto;
                        deliveryDto.ReceiptInfo = receiptDto;

                        return Ok(deliveryDto);
                    }
                case (ReceiptMethodEnum.Pickup):
                    {
                        var receiptDomain = await pickupReceiptRepository.GetByDeliveryIdAsync(id);
                        if (receiptDomain == null)
                        {
                            return NotFound();
                        }

                        var receiptDto = mapper.Map<PickupReceiptDto>(receiptDomain);
                        var deliveryDto = mapper.Map<DeliveryDto<PickupReceiptDto>>(deliveryDomain);
                        deliveryDto.Order = orderDto;
                        deliveryDto.ReceiptInfo = receiptDto;

                        return Ok(deliveryDto);
                    }
                default: return BadRequest();
            }

        }

        [Authorize]
        [HttpGet("{orderId:Guid}")]
        public async Task<IActionResult> GetByOrderId([FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> SetCancelStatus([FromRoute] Guid id, [FromBody] SetCancelStatusDto cancelStatusDto)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> SetReceiveStatus([FromRoute] Guid id, [FromBody] SetReceiveStatusDto receiveStatusDto)
        {
            throw new NotImplementedException();
        }
    }
}
