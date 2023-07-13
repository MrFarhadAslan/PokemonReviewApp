using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class ReviewerRepository : IReviewerRepository
	{
		private readonly DataContext _dataContext;

		public ReviewerRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}
        public bool CreateReviewer(Reviewer reviewer)
		{
			_dataContext.Reviewers.Add(reviewer);
			return Save();
		}

		public bool DeleteReviewer(Reviewer reviewer)
		{
			_dataContext.Reviewers.Remove(reviewer);
			return Save();
		}

		public Reviewer GetReviewer(int id)
		{
			return _dataContext.Reviewers.Where(x => x.Id == id).FirstOrDefault();
		}

		public ICollection<Reviewer> GetReviewers()
		{
			return _dataContext.Reviewers.ToList();
		}

		public ICollection<Review> GetReviewsByReviewer(int reviewerId)
		{
			return _dataContext.Reviewers
						.Include(x=>x.Reviews)
						.Where(x => x.Id == reviewerId).FirstOrDefault()?.Reviews;
		}

		public bool ReviewerExists(int reviewerId)
		{
			return _dataContext.Reviewers.Any(x => x.Id == reviewerId);
		}

		public bool Save()
		{
			return _dataContext.SaveChanges() > 0 ? true : false;
		}

		public bool UpdateReviewer(Reviewer reviewer)
		{
			_dataContext.Reviewers.Update(reviewer);
			return Save();
		}
	}
}
