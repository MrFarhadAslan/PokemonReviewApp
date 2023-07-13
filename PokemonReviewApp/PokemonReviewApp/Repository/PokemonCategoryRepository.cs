using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class PokemonCategoryRepository:IPokemonCategoryRepository
	{
		private readonly DataContext _dataContext;

		public PokemonCategoryRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

		public bool Create(PokemonCategory category)
		{
			_dataContext.PokemonCategories.Add(category);
			return Save();	
		}

		public bool Delete(PokemonCategory pokemonCategory)
		{
			_dataContext.PokemonCategories.Remove(pokemonCategory);
			return Save();
		}

		public PokemonCategory GetPokemonCategoryByIds(int pokiId, int categoryId)
		{
			return _dataContext.PokemonCategories.Where(x => x.CategoryId == categoryId && x.PokemonId == pokiId).FirstOrDefault();
		}

		public bool PokemonCategoryExist(int pokiId, int categoryId)
		{
			return _dataContext.PokemonCategories.Where(x=>x.CategoryId == categoryId && x.PokemonId == pokiId).Any();
		}

		public bool Save()
		{
			var result = _dataContext.SaveChanges();
			return result > 0 ? true: false;
		}
	}
}
