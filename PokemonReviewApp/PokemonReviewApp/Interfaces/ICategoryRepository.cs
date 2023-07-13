using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface ICategoryRepository
	{
		ICollection<Category> GetCategories();
		Category GetCategory(int id);
		ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
		bool CategoryExists(int id);
		bool Create(Category category);
		bool Update(Category category);
		bool Delete(Category category);
		bool Save();
	}
}
