using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		private readonly IGlobalRepository _globalRepository;

		public CategoryController(ICategoryRepository categoryRepository,IMapper mapper,IGlobalRepository globalRepository)
        {
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_globalRepository = globalRepository;
		}

		[HttpGet]
		[Route("GetCategory/categoryId")]
		[ProducesResponseType(200 , Type = typeof(CategoryDto))]
		public IActionResult GetCategory(int id)
		{
			if (!_categoryRepository.CategoryExists(id)) return NotFound();

			return Ok(_mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id)));
		}

		[HttpGet("GetCategories")]
		[ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
		public IActionResult GetCategories()
		{
			return Ok(_mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories()));
		}

		[HttpGet("GetPokemonsByCategory/categoryId")]
		[ProducesResponseType(200 , Type = typeof(List<PokemonDto>))]
		public IActionResult GetPokemonsByCategory(int categoryId)
		{
			return Ok(_mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId)));
		}

		[HttpPost("Create")]
		public IActionResult Create([FromBody] CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newCategory = _mapper.Map<Category>(categoryDto);

			if (!_categoryRepository.Create(newCategory))
				return StatusCode(400, "An error occurred while careting a category");

			return Ok("Succeesfuly created category");
		}

		[HttpPut]
		public IActionResult Update([FromQuery] int categoryId, [FromBody]  CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_categoryRepository.CategoryExists(categoryId))
				return NotFound("Category not found!");

			var existCategory = _categoryRepository.GetCategory(categoryId);
			_globalRepository.StopTrackingEntity(existCategory);

			existCategory = _mapper.Map<Category>(categoryDto);
			existCategory.Id = categoryId;

			if (!_categoryRepository.Update(existCategory))
				return StatusCode(400, "An error occurred while updating a category");
			//if (!_globalRepository.GlobalSave())
			//	return StatusCode(400, "An error occurred while updating a category");

			return Ok("Succeesfuly updated category");

		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			if (!_categoryRepository.CategoryExists(id))
				return NotFound("Category not found!");

			if (!_categoryRepository.Delete(_categoryRepository.GetCategory(id)))
				return StatusCode(400, "An error occurred while deleting a category");

			return Ok("Succceesfuly deleted!");
		}
    }
}
