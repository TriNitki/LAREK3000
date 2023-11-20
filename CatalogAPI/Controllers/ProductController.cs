using AutoMapper;
using CatalogAPI.Models.Domain;
using CatalogAPI.Models.DTO.ProductDTO;
using CatalogAPI.Repositories.IRepositories;
using CatalogAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IAuthService authService;

        public ProductController(IProductRepository productRepository, IMapper mapper, IAuthService authService)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var productDomains = await productRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            var productDtos = mapper.Map<List<ReducedProductDto>>(productDomains);
            return Ok(productDtos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var productDomain = await productRepository.GetByIdAsync(id);

            if (productDomain == null)
            {
                return NotFound();
            }

            var userRequest = await authService.GetUserByIdAsync(productDomain.SellerId);
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest();
            }
            var userDto = userRequest.Result;

            var productDto = mapper.Map<ProductDto>(productDomain);
            productDto.Seller = userDto;
            return Ok(productDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var userRequest = await authService.GetUserByIdAsync(Guid.Parse(userId));

            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest();
            }
            var userDto = userRequest.Result;


            var productModel = mapper.Map<Product>(createProductDto);
            productModel.SellerId = userDto.Id;

            var productDomain = await productRepository.CreateAsync(productModel);

            var productDto = mapper.Map<ProductDto>(productDomain);

            productDto.Seller = userDto;

            return Ok(productDto);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            var productModel = mapper.Map<Product>(updateProductDto);
            var productDomain = await productRepository.UpdateAsync(id, productModel);

            if (productDomain == null)
            {
                return NotFound();
            }

            var userRequest = await authService.GetUserByIdAsync(productDomain.SellerId);
            if (userRequest == null || !userRequest.IsSuccess || userRequest.Result == null)
            {
                return BadRequest();
            }
            var userDto = userRequest.Result;

            var productDto = mapper.Map<ProductDto>(productDomain);
            productDto.Seller = userDto;
            
            return Ok(productDto);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var productDomain = await productRepository.DeleteAsync(id);

            if (productDomain == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
