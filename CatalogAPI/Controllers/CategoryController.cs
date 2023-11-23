using AutoMapper;
using CatalogAPI.Models.Domain;
using CatalogAPI.Models.DTO.CategoryDTO;
using CatalogAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var categoryDomain = await categoryRepository.GetAsync(id);

            if (categoryDomain == null)
            {
                return NotFound();
            }

            var domainDto = mapper.Map<CategoryDto>(categoryDomain);
            return Ok(domainDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var categoryDomains = await categoryRepository.GetAllAsync(sortBy, isAscending ?? true);

            var categoryDtos = mapper.Map<List<CategoryDto>>(categoryDomains);
            return Ok(categoryDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var categoryModel = mapper.Map<Category>(createCategoryDto);

            var categoryDomain = await categoryRepository.CreateAsync(categoryModel);

            var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

            return CreatedAtAction(nameof(Get), new { id = categoryDomain.Id }, categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> UpdateName([FromRoute] Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var categoryDomain = await categoryRepository.UpdateNameAsync(id, updateCategoryDto.Name);

            var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var isDeleted = await categoryRepository.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
