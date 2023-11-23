using AutoMapper;
using CatalogAPI.Models.Domain;
using CatalogAPI.Models.DTO.BrandDTO;
using CatalogAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository brandRepository;
        private readonly IMapper mapper;

        public BrandController(IBrandRepository brandRepository, IMapper mapper)
        {
            this.brandRepository = brandRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var brandDomain = await brandRepository.GetAsync(id);

            if (brandDomain == null)
            {
                return NotFound();
            }

            var domainDto = mapper.Map<BrandDto>(brandDomain);
            return Ok(domainDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var brandDomains = await brandRepository.GetAllAsync(sortBy, isAscending ?? true);

            var brandDtos = mapper.Map<List<BrandDto>>(brandDomains);
            return Ok(brandDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandDto createBrandDto)
        {
            var brandModel = mapper.Map<Brand>(createBrandDto);

            var brandDomain = await brandRepository.CreateAsync(brandModel);

            var brandDto = mapper.Map<BrandDto>(brandDomain);

            return CreatedAtAction(nameof(Get), new { id = brandDomain.Id }, brandDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> UpdateName([FromRoute] Guid id, [FromBody] UpdateBrandNameDto updateBrandDto)
        {
            var brandDomain = await brandRepository.UpdateNameAsync(id, updateBrandDto.Name);

            var brandDto = mapper.Map<BrandDto>(brandDomain);

            return Ok(brandDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var isDeleted = await brandRepository.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
