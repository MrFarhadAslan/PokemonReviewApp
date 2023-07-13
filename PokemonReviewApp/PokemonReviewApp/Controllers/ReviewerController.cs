using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewerController : ControllerBase
	{
		private readonly IReviewerRepository _reviewerRepository;
		private readonly IMapper _mapper;
		private readonly IGlobalRepository _globalRepository;

		public ReviewerController(IReviewerRepository reviewerRepository,IMapper mapper,IGlobalRepository globalRepository)
        {
			_reviewerRepository = reviewerRepository;
			_mapper = mapper;
			_globalRepository = globalRepository;
		}

		[HttpGet("GetReviewers")]
		[ProducesResponseType(200,Type = typeof(List<ReviewerDto>))]
		public IActionResult GetReviewers()
		{
			return Ok(_mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers()));
		}

		[HttpGet("GetReviewer/reviewerId")]
		[ProducesResponseType(200, Type = typeof(ReviewerDto))]
		public IActionResult GetReviewer(int reviewerId)
		{
			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound("Reviewr not found!");

			return Ok(_mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId)));
		}

		[HttpGet("GetReviewsByReviewer/reviewerId")]
		[ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
		public IActionResult GetReviewsByReviewer(int reviewerId)
		{
			return Ok(_mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId)));
		}

		[HttpPost("Create")]
		public IActionResult Create([FromBody] ReviewerDto reviewer)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var newReviewer = _mapper.Map<Reviewer>(reviewer);
			if(!_reviewerRepository.CreateReviewer(newReviewer))
				return StatusCode(400, "An error occurred while careting a reviewer");

			return Ok("Succeesfuly created!");
		}

		[HttpPut("Update")]
		public IActionResult Update([FromBody] ReviewerDto reviewerDto, [FromQuery] int reviewerId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound("Reviewer not found!");

			var existReviewer = _reviewerRepository.GetReviewer(reviewerId);
			existReviewer = _mapper.Map<Reviewer>(reviewerDto);
			_globalRepository.StopTrackingEntity(existReviewer);

			if(!_reviewerRepository.UpdateReviewer(existReviewer))
				return StatusCode(400, "An error occurred while updating a reviewer");

			return Ok("Succeesfuly updated!");
		}

		[HttpDelete("Delete/reviewerId")]
		public IActionResult Delete(int reviewerId)
		{
			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound("Reviewer not found!");

			if(!_reviewerRepository.DeleteReviewer(_reviewerRepository.GetReviewer(reviewerId)))
				return StatusCode(400, "An error occurred while deleting a reviewer");

			return Ok("Succeesfuly deleted!");
		}
	}
}
