using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IReviewerRepository
	{
		bool ReviewerExists(int reviewerId);
		Reviewer GetReviewer(int id);
		ICollection<Reviewer> GetReviewers();
		ICollection<Review> GetReviewsByReviewer(int reviewerId);
		bool CreateReviewer(Reviewer reviewer);
		bool UpdateReviewer(Reviewer reviewer);
		bool DeleteReviewer(Reviewer reviewer);
		bool Save();

	}
}
