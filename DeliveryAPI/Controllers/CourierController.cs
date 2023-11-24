using AutoMapper;
using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;
using DeliveryAPI.Repositories.IRepositories;
using DeliveryAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierReceiptRepository courierReceiptRepository;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public CourierController(ICourierReceiptRepository courierReceiptRepository, IAuthService authService, IMapper mapper)
        {
            this.courierReceiptRepository = courierReceiptRepository;
            this.authService = authService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Courier")]
        [HttpGet]
        public async Task<IActionResult> GetCourierDeliveries([FromQuery] bool sortByRecent = true, [FromQuery] bool includeDelivered = false)
        {
            var courierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (courierId == null)
            {
                return Unauthorized();
            }

            var courierResponse = await authService.GetUserByIdAsync(Guid.Parse(courierId));
            if (courierResponse == null)
            {
                return NotFound();
            }

            var courierDto = courierResponse.Result;

            var courierDeliveriesDomain = await courierReceiptRepository.GetByCourierIdAsync(Guid.Parse(courierId), sortByRecent, includeDelivered);
            var courierDeliveriesDto = mapper.Map<List<ReducedCourierReceiptDto>>(courierDeliveriesDomain);

            var response = new ResponseCourierReceipts()
            {
                Courier = courierDto,
                Receipts = courierDeliveriesDto
            };

            return Ok(response);
        }

        [Authorize(Roles = "Courier")]
        [HttpGet]
        public async Task<IActionResult> GetCourierProfit([FromQuery] DateTime? deliveriesFrom, [FromQuery] DateTime? deliveriesTo)
        {
            var courierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (courierId == null)
            {
                return Unauthorized();
            }

            var courierProfit = await courierReceiptRepository.CalculateCourierProfitAsync(Guid.Parse(courierId), deliveriesFrom, deliveriesTo);

            var response = new CourierProfitDto()
            {
                profit = courierProfit,
            };

            return Ok(response);
        }

        [Authorize(Roles = "Courier")]
        [HttpPatch("{deliveryId:Guid}")]
        public async Task<IActionResult> SetDeliveryStatus([FromRoute] Guid deliveryId, [FromBody] SetDeliveryStatusDto deliveryStatusDto)
        {
            var courierId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (courierId == null)
            {
                return Unauthorized();
            }

            var courierReceiptDomain = await courierReceiptRepository.SetDeliveryStatus(deliveryId, deliveryStatusDto.IsDelivered);

            if (courierReceiptDomain == null)
            {
                return BadRequest();
            }

            var courierReceiptDto = mapper.Map<CourierReceiptDto>(courierReceiptDomain);

            return Ok(courierReceiptDto);
        }
    }
}
