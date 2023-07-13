using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PokemonController : ControllerBase
	{
		private readonly IPokemonRepository _pokemonRepository;
		private readonly IMapper _mapper;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IOwnerRepository _ownerRepository;
		private readonly IPokemonCategoryRepository _pokemonCategoryRepository;
		private readonly IGlobalRepository _globalRepository;
		private readonly IPokemonOwnerRepository _pokemonOwnerRepository;

		public PokemonController(IPokemonRepository pokemonRepository,IMapper mapper,ICategoryRepository categoryRepository,IOwnerRepository ownerRepository,IPokemonCategoryRepository pokemonCategoryRepository,IGlobalRepository globalRepository,IPokemonOwnerRepository pokemonOwnerRepository)
        {
			_pokemonRepository = pokemonRepository;
			_mapper = mapper;
			_categoryRepository = categoryRepository;
			_ownerRepository = ownerRepository;
			_pokemonCategoryRepository = pokemonCategoryRepository;
			_globalRepository = globalRepository;
			_pokemonOwnerRepository = pokemonOwnerRepository;
		}

		[HttpGet("pokemons")]
		[ProducesResponseType(200 ,Type = typeof(IEnumerable<PokemonDto>))]
		public IActionResult GetPokemons()
		{
			var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

			return Ok(pokemons);
		}

		[HttpGet("GetPokemon/pokiId")]
		[ProducesResponseType(200,Type = typeof(PokemonDto))]
		public IActionResult GetPokemonById(int id)
		{
			var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));

			if(pokemon == null) return NotFound();

			return Ok(pokemon);
		}

		[HttpGet("PokemonReviews/pokiId")]
		[ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
		public IActionResult GetPokemonReview(int pokeId)
		{
			return Ok(_mapper.Map<List<ReviewDto>>(_pokemonRepository.GetPokemonReview(pokeId)));
		}

		[HttpPost("PokemonUpdate/pokeId")]
		[ProducesResponseType(200)]
		public IActionResult Create([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto createPokemon)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (createPokemon == null) return BadRequest(ModelState);

			if (!_categoryRepository.CategoryExists(categoryId)) return NotFound("Sorry category not found!");
			if (!_ownerRepository.OwnerExist(ownerId)) return NotFound("Sorry owner not found!");

			var checkPokemon = _pokemonRepository.GetPokemons()
									.Where(x => x.Name.Trim().ToUpper() == createPokemon.Name.Trim().ToUpper()).FirstOrDefault();

			if (checkPokemon != null)
			{
				ModelState.AddModelError("", "Owner already exists");
				return StatusCode(422, ModelState);
			}

			var pokemon = _mapper.Map<Pokemon>(createPokemon);

			if(!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
			{
				ModelState.AddModelError("", "Something went wrong while savin!");
				return StatusCode(500,ModelState);
			}

			return Ok("Successfully created");
		}
		[HttpPut("PokemonUpdate/pokeId")]
		[ProducesResponseType(200)]
		public IActionResult Update([FromQuery] int pokiId,[FromBody] PokemonDto updatePokemonDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			if (!_pokemonRepository.PokemonExists(pokiId)) return NotFound("Sorry, Pokemon not found!");

			var pokemon = _mapper.Map<Pokemon>(updatePokemonDto);
			pokemon.Id = pokiId;

			if (!_pokemonRepository.UpdatePokemon(pokemon))
			{
				ModelState.AddModelError("", "Something went wrong while update!");
				return BadRequest(ModelState);
			}

			return Ok("Succeesfully updated");
		}

		[HttpPut("PokemonCategoryUpdate")]
		public IActionResult PokemonCategoryUpdate(int oldPokiCategoryId, int pokiId, int newPokiCategory)
		{

			if (!_pokemonCategoryRepository.PokemonCategoryExist(pokiId, oldPokiCategoryId)) return NotFound("There is no such connection");

			if (!_categoryRepository.CategoryExists(newPokiCategory)) return NotFound("New category not found!");
			var checkPokiCategory = _pokemonCategoryRepository.GetPokemonCategoryByIds(pokiId, newPokiCategory);
			if (checkPokiCategory != null)
			{
				//ModelState.AddModelError("", "there is already a relationship");
				//return BadRequest(ModelState);
				return StatusCode(400, "there is already a relationship");
			}

			var pokiCategory = _pokemonCategoryRepository.GetPokemonCategoryByIds(pokiId, oldPokiCategoryId);
			_pokemonCategoryRepository.Delete(pokiCategory);

			PokemonCategory updatePokiCategory = new PokemonCategory
			{
				PokemonId = pokiId,
				CategoryId = newPokiCategory
			};

			if (!_pokemonCategoryRepository.Create(updatePokiCategory))
			{
				ModelState.AddModelError("", "Unexpected error");
				return BadRequest(ModelState);
			}


			return Ok("Succeesfully pokemon category updated");

		}

		[HttpPut("PokemonOwnerUpdate")]
		public IActionResult PokemonOwnerUpdate(int oldOwnerId,int pokiId,int newOwnerId)
		{
			if (!_pokemonOwnerRepository.PokemonOwnerExist(pokiId, oldOwnerId)) return NotFound("There is no such connection");
			if (!_ownerRepository.OwnerExist(newOwnerId)) return NotFound("New owner not found");

			var checkPokiOwner = _pokemonOwnerRepository.PokemonOwnerExist(pokiId, newOwnerId);
			if (checkPokiOwner)
				return BadRequest("There is already a relationship");

			var pokemonOwner = _pokemonOwnerRepository.GetPokemonOwnerByIds(pokiId,oldOwnerId);
			_pokemonOwnerRepository.Delete(pokemonOwner);

			PokemonOwner newPokiOwner = new PokemonOwner
			{
				PokemonId = pokiId,
				OwnerId = newOwnerId
			};
			if (!_pokemonOwnerRepository.Create(newPokiOwner))
			{
				return StatusCode(404, "Something went wrong while update!");
			}

			return Ok("Succeesfully pokemon owner updated");
		}

		[HttpDelete]
		public IActionResult Delete(int pokiId)
		{
			if (!_pokemonRepository.PokemonExists(pokiId)) return NotFound("Pokemon Not Found");

			if (!_pokemonRepository.DeletePokemon(_pokemonRepository.GetPokemon(pokiId)))
				return StatusCode(404, "Something went wrong while delete!");

			return Ok("Succeesfully pokemon deleted!");

		}

	}
}
