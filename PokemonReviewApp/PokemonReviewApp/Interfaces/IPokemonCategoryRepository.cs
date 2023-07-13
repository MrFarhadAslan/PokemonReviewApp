using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IPokemonCategoryRepository
	{
		bool PokemonCategoryExist(int pokiId,int categoryId);
		PokemonCategory GetPokemonCategoryByIds(int pokiId,int categoryId);
		bool Create(PokemonCategory category);
		bool Delete(PokemonCategory pokemonCategory);
		bool Save();

	}
}
