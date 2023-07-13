using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class ReviewRepository:IReviewRepository
	{
		private readonly DataContext _dataContext;

		public ReviewRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

		public bool Create(Review review)
		{
			_dataContext.Reviews.Add(review);
			return Save();
		}

		public bool Delete(Review review)
		{
			_dataContext.Reviews.Remove(review);
			return Save();
		}

		public Review GetReview(int id)
		{
			return _dataContext.Reviews.FirstOrDefault(x=>x.Id == id);
		}

		public ICollection<Review> GetReviews()
		{
			return _dataContext.Reviews.ToList();
		}

		public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
		{
			return _dataContext.Reviews.Where(x=>x.PokemonId == pokeId).ToList();
		}

		public bool ReviewExist(int id)
		{
			return _dataContext.Reviews.Where(x=>x.Id == id).Any();
		}

		public bool Save()
		{
			return _dataContext.SaveChanges() > 0 ? true : false;
		}

		public bool Update(Review review)
		{
			_dataContext.Reviews.Update(review);
			return Save();
		}
	}
}
