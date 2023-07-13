using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OwnerController : ControllerBase
	{
		private readonly IOwnerRepository _ownerRepository;
		private readonly IMapper _mapper;
		private readonly IPokemonRepository _pokemonRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly IGlobalRepository _globalRepository;

		public OwnerController(IOwnerRepository ownerRepository,IMapper mapper,IPokemonRepository pokemonRepository,ICountryRepository countryRepository,IGlobalRepository globalRepository)
        {
			_ownerRepository = ownerRepository;
			_mapper = mapper;
			_pokemonRepository = pokemonRepository;
			_countryRepository = countryRepository;
			_globalRepository = globalRepository;
		}

		[HttpGet("GetOwner/ownerId")]
		[ProducesResponseType(200,Type = typeof(OwnerDto))]
		public IActionResult GetOwner(int ownerId)
		{
			if (!_ownerRepository.OwnerExist(ownerId))
				return NotFound("Owner not found!");

			return Ok(_mapper.Map<OwnerDto>(_ownerRepository.GetOwnerById(ownerId)));
		}

		[HttpGet("GetOwners")]
		[ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
		public IActionResult GetOwners()
		{
			return Ok(_mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners()));
		}

		[HttpGet("GetOwnerOfAPokemon/pokiId")]
		[ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
		public IActionResult GetOwnerOfAPokemon(int pokiId)
		{
			if (!_pokemonRepository.PokemonExists(pokiId))
				return NotFound("Pokemon not found!");

			return Ok(_mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokiId)));
		}

		[HttpGet("GetPokemonByOwner/ownerId")]
		[ProducesResponseType(200,Type = (typeof(ICollection<PokemonDto>)))]
		public IActionResult GetPokemonByOwner(int ownerId)
		{
			if (!_ownerRepository.OwnerExist(ownerId))
				return NotFound("Owner not found!");

			return Ok(_mapper.Map<ICollection<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId)));
		}

		[HttpPost("Create")]
		public IActionResult Create([FromBody] OwnerDto ownerDto, [FromQuery] int countryId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var owner = _ownerRepository.GetOwners()
							.Where(x => x.Name.Trim().ToLower() == ownerDto.Name.Trim().ToLower()).FirstOrDefault();
			if (owner != null)
				return BadRequest("Owner already exists");
			
			if (!_countryRepository.ExistCountry(countryId))
				return NotFound("Country not found!");

			Owner newOwner = _mapper.Map<Owner>(ownerDto);
			newOwner.CountryId = countryId;

			if (!_ownerRepository.Create(newOwner))
				return StatusCode(400, "An error occurred while careting a owner");

			return Ok("Succeesfuly created!");
		}

		[HttpPut("Update")]
		public IActionResult Update([FromBody] OwnerDto ownerDto, [FromQuery] int ownerId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existOwner = _ownerRepository.GetOwnerById(ownerId);
			_globalRepository.StopTrackingEntity(existOwner);

			if (existOwner == null)
				return NotFound("Owner not found!");

			existOwner = _mapper.Map<Owner>(ownerDto);
			existOwner.Id = ownerId;
			if (!_countryRepository.ExistCountry(ownerDto.CountryId))
				return NotFound("Country not found!");
			existOwner.CountryId = ownerDto.CountryId;

			if (!_ownerRepository.Update(existOwner))
				return StatusCode(400, "An error occurred while updating a owner");

			return Ok("Succeesfuly updated!");

		}

		[HttpDelete("Delete/ownerId")]
		public IActionResult Delete(int ownerId)
		{
			if (!_ownerRepository.OwnerExist(ownerId))
				return NotFound("Owner not found!");
			
			if(!_ownerRepository.Delete(_ownerRepository.GetOwnerById(ownerId)))
				return StatusCode(400, "An error occurred while deleting a owner");

			return Ok("Succeesfuly deleted!");
		}
	}
}
