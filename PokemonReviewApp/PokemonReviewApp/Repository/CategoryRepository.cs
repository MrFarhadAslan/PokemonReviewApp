using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext _dataContext;

		public CategoryRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

        public bool CategoryExists(int id)
		{
			return _dataContext.Categories.Any(c => c.Id == id);
		}

		public bool Create(Category category)
		{
			_dataContext.Categories.Add(category);
			return Save();
		}

		public bool Delete(Category category)
		{
			_dataContext.Categories.Remove(category);
			return Save();
		}

		public ICollection<Category> GetCategories()
		{
			return _dataContext.Categories.ToList();
		}

		public Category GetCategory(int id)
		{
			return _dataContext.Categories.FirstOrDefault(c => c.Id == id);
		}

		public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
		{
			return _dataContext.PokemonCategories.Where(x=>x.CategoryId == categoryId).Select(x=>x.Pokemon).ToList();
		}

		public bool Save()
		{
			return _dataContext.SaveChanges() > 0 ? true : false;
		}

		public bool Update(Category category)
		{
			_dataContext.Categories.Update(category);
			return Save();
		}


	}
}
