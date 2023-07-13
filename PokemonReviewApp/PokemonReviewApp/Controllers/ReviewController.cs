using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewRepository _reviewRepository;
		private readonly IMapper _mapper;
		private readonly IGlobalRepository _globalRepository;

		public ReviewController(IReviewRepository reviewRepository,IMapper mapper,IGlobalRepository globalRepository)
        {
			_reviewRepository = reviewRepository;
			_mapper = mapper;
			_globalRepository = globalRepository;
		}

		[HttpGet("GetReviews")]
		[ProducesResponseType(200,Type = typeof(ICollection<ReviewDto>))]
		public IActionResult GetReviews()
		{
			ICollection<ReviewDto> reviewDtos = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

			return Ok(reviewDtos);
		}

		[HttpGet("GetReview/Id")]
		[ProducesResponseType(200, Type = typeof(ReviewDto))]
		public IActionResult GetReview(int id)
		{
			if (!_reviewRepository.ReviewExist(id))
				return NotFound("Review Not Found!");

			var reviewDto = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(id));

			return Ok(reviewDto);
		}

		[HttpGet("GetReviewsOfAPokemon/pokiId")]
		[ProducesResponseType(200,Type = typeof(ICollection<ReviewDto>))]
		public IActionResult GetReviewsOfAPokemon(int pokiId)
		{
			var reviews = _reviewRepository.GetReviewsOfAPokemon(pokiId);

			return Ok(_mapper.Map<ReviewDto>(reviews));
		}

		[HttpPost("Create")]
		public IActionResult Create([FromBody]ReviewDto reviewDto, [FromQuery] int pokiId,[FromQuery]int reviewerId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var review = _reviewRepository.GetReviews()
							.Where(x => x.Title.Trim().ToLower() == reviewDto.Title.ToLower()).FirstOrDefault();

			if (review != null)
				return BadRequest("Review has taken!");

			var newReview = _mapper.Map<Review>(reviewDto);
			newReview.ReviewerId = reviewerId;
			newReview.PokemonId = pokiId;

			if (!_reviewRepository.Create(newReview))
				return StatusCode(400, "An error occurred while creating a review");

			return Ok("Succeesfuly review created!");
		}

		[HttpPut("Update")]
		[ProducesResponseType(200)]
		public IActionResult Update([FromBody]ReviewDto reviewDto, [FromQuery] int reviewId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_reviewRepository.ReviewExist(reviewId))
				return NotFound("Review not found");

			var existReview = _reviewRepository.GetReview(reviewId);
			_globalRepository.StopTrackingEntity(existReview);

			var reviewUpdate = _mapper.Map<Review>(reviewDto);
			reviewUpdate.Id = reviewId;
			reviewUpdate.PokemonId = existReview.PokemonId;
			reviewUpdate.ReviewerId = existReview.ReviewerId;

			if (!_reviewRepository.Update(reviewUpdate))
				return StatusCode(400, "An error occurred while updateing a review");

			return Ok("Succeesfuly review updated!");
		}

		[HttpDelete("Delete/Id")]
		public IActionResult Delete(int  reviewId)
		{
			if (!_reviewRepository.ReviewExist(reviewId))
				return NotFound("Review not found");

			if (!_reviewRepository.Delete(_reviewRepository.GetReview(reviewId)))
				return StatusCode(400, "An error occurred while deleting a review");

			return Ok("Succeesfuly review deleted!");
		}
	}
}
