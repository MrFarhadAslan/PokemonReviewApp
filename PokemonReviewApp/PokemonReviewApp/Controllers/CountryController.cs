using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;
		private readonly IGlobalRepository _globalRepository;

		public CountryController(ICountryRepository countryRepository,IMapper mapper,IGlobalRepository globalRepository)
        {
			_countryRepository = countryRepository;
			_mapper = mapper;
			_globalRepository = globalRepository;
		}

		[HttpGet("GetCountries")]
		[ProducesResponseType(200, Type = typeof(ICollection<CountryDto>))]
		public IActionResult GetCountries()
		{
			return Ok(_mapper.Map<ICollection<CountryDto>>(_countryRepository.GetCountries()));
		}

		[HttpGet("GetCountry/Id")]
		[ProducesResponseType(200, Type = typeof(CountryDto))]
		public IActionResult GetCountry(int countryId)
		{
			if (!_countryRepository.ExistCountry(countryId))
				return NotFound("Country not found!");

			return Ok(_mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId)));
		}

		[HttpGet("GetOwnersFromACountry/countryId")]
		[ProducesResponseType(200, Type = typeof(ICollection<OwnerDto>))]
		public IActionResult GetOwnersFromACountry(int countryId)
		{
			if (!_countryRepository.ExistCountry(countryId))
				return NotFound("Country not found!");

			return Ok(_mapper.Map<ICollection<OwnerDto>>(_countryRepository.GetOwnersFromACountry(countryId)));
		}

		[HttpPost("Create")]
		public IActionResult Create(CountryDto countryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newCountry = _mapper.Map<Country>(countryDto);

			if (!_countryRepository.Create(newCountry))
				return StatusCode(400, "An error occurred while careting a country");

			return Ok("Succeesfuly created country");
		}

		[HttpPut("Update")]
		public IActionResult Upadte([FromBody]CountryDto countryDto, [FromQuery] int countryId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_countryRepository.ExistCountry(countryId))
				return NotFound("Country not found!");

			var existCountry = _countryRepository.GetCountry(countryId);
			_globalRepository.StopTrackingEntity(existCountry);

			existCountry = _mapper.Map<Country>(countryDto);
			existCountry.Id = countryId;

			if(!_countryRepository.Update(existCountry))
				return StatusCode(400, "An error occurred while careting a country");

			return Ok("Succeesfuly updated country");
		}

		[HttpDelete("Delete/countryId")]
		public IActionResult Delete(int countryId)
		{
			if (!_countryRepository.ExistCountry(countryId))
				return NotFound("Country not found!");

			if (!_countryRepository.Delete(_countryRepository.GetCountry(countryId)))
				return StatusCode(400, "An error occurred while careting a country");

			return Ok("Succeesfuly deleted country");
		}
	}
}
