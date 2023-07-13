using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IReviewRepository
	{
		bool ReviewExist(int id);
		Review GetReview(int id);
		ICollection<Review> GetReviews();
		ICollection<Review> GetReviewsOfAPokemon(int pokeId);
		bool Create(Review review);
		bool Update(Review review);
		bool Delete(Review review);
		bool Save();

	}
}
