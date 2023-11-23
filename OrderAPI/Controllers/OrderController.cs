using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using OrderAPI.Models.Domain;
using OrderAPI.Models.DTO.DeliveryDTO;
using OrderAPI.Models.DTO.OrderDTO;
using OrderAPI.Models.Enum;
using OrderAPI.Repositories.IRepositories;
using OrderAPI.Service.IService;
using System.Security.Claims;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        private readonly ICatalogService catalogService;
        private readonly IDeliveryService delivery;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IAuthService authService, ICatalogService catalogService, IDeliveryService delivery)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.authService = authService;
            this.catalogService = catalogService;
            this.delivery = delivery;
        }

        [HttpGet("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            // Get order from db
            var orderDomain = await orderRepository.GetByIdAsync(id);
            if (orderDomain == null)
            {
                Console.WriteLine("order domain");
                return NotFound();
            }

            // Get product from catalog service
            var productRequest = await catalogService.GetProductByIdAsync(orderDomain.ProductId);
            if (productRequest == null || !productRequest.IsSuccess || productRequest.Result == null)
            {
                Console.WriteLine("product request");
                return BadRequest();
            }
            var productDto = productRequest.Result;

            // Get buyer from auth service
            var userRequest = await authService.GetUserByIdAsync(orderDomain.BuyerId);
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                Console.WriteLine("user request");
                return BadRequest();
            }
            var userDto = userRequest.Result;

            // Map and insert data from external services
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            orderDto.Product = productDto;
            orderDto.Buyer = userDto;

            return Ok(orderDto);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] bool? includeCanceled)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var orderDomains = await orderRepository.GetAllByUserIdAsync(Guid.Parse(userId), includeCanceled ?? true);
            var orderDtos = mapper.Map<List<ReducedOrderDto>>(orderDomains);
            return Ok(orderDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            // Get user id from session
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("No user id inside of session");
            }

            // Get access token
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (accessToken.ToString() == null)
            {
                return Unauthorized("Invalid access token");
            }

            // Get buyer from auth service
            var userRequest = await authService.GetUserByIdAsync(Guid.Parse(userId));
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest("Can't find user");
            }
            var userDto = userRequest.Result;

            // Get product from catalog service
            var productRequest = await catalogService.GetProductByIdAsync(createOrderDto.ProductId);
            if (productRequest == null || !productRequest.IsSuccess || productRequest.Result == null)
            {
                return BadRequest("Can't find product");
            }
            var productDto = productRequest.Result;

            // Save order in db
            var orderModel = mapper.Map<Order>(createOrderDto);
            orderModel.BuyerId = userDto.Id;

            var orderDomain = await orderRepository.CreateAsync(orderModel);

            var createDeliveryDto = new CreateDeliveryDto()
            {
                OrderId = orderDomain.Id,
                Address = createOrderDto.Delivery.Address,
                ReceiptMethod = createOrderDto.Delivery.ReceiptMethod
            };

            switch (createDeliveryDto.ReceiptMethod)
            {
                case (ReceiptMethodEnum.Courier):
                    {
                        var responseDelivery = await delivery.CreateCourierDelivery(createDeliveryDto, accessToken);

                        if (responseDelivery == null || !responseDelivery.IsSuccess || responseDelivery.Result == null)
                        {
                            return BadRequest("Cant create delivery");
                        }
                        break;
                    }
                case (ReceiptMethodEnum.Pickup):
                    {
                        var responseDelivery = await delivery.CreatePickupDelivery(createDeliveryDto, accessToken);
                        Console.WriteLine(responseDelivery.IsSuccess);
                        Console.WriteLine(responseDelivery.Result.ShippingDT);

                        if (responseDelivery == null || !responseDelivery.IsSuccess || responseDelivery.Result == null)
                        {
                            return BadRequest("Can't create delivery");
                        }
                        break;
                    }
                default: return BadRequest("Invalid delivery receipt method");
            }

            // Map and insert data from external services
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            orderDto.Product = productDto;
            orderDto.Buyer = userDto;

            return Ok(orderDto);
        }

        [HttpPatch("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelOrderDto cancelOrderDto)
        {
            var orderDomain = await orderRepository.SetCancelStatusAsync(id, cancelOrderDto.IsCanceled);
            if (orderDomain == null)
            {
                return NotFound();
            }

            // Get buyer from auth service
            var userRequest = await authService.GetUserByIdAsync(orderDomain.BuyerId);
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest();
            }
            var userDto = userRequest.Result;

            // Get product from catalog service
            var productRequest = await catalogService.GetProductByIdAsync(orderDomain.ProductId);
            if (productRequest == null || !productRequest.IsSuccess || productRequest.Result == null)
            {
                return BadRequest();
            }
            var productDto = productRequest.Result;

            // Map and insert data from external services
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            orderDto.Product = productDto;
            orderDto.Buyer = userDto;

            return Ok(orderDto);
        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var orderModel = mapper.Map<Order>(updateOrderDto);
            var orderDomain = await orderRepository.UpdateAsync(id, orderModel);
            if (orderDomain == null)
            {
                return NotFound();
            }

            // Get buyer from auth service
            var userRequest = await authService.GetUserByIdAsync(orderDomain.BuyerId);
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest();
            }
            var userDto = userRequest.Result;

            // Get product from catalog service
            var productRequest = await catalogService.GetProductByIdAsync(orderDomain.ProductId);
            if (productRequest == null || !productRequest.IsSuccess || productRequest.Result == null)
            {
                return BadRequest();
            }
            var productDto = productRequest.Result;

            // Map and insert data from external services
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            orderDto.Product = productDto;
            orderDto.Buyer = userDto;

            return Ok(orderDto);
        }
    }
}
