using PokemonReviewApp.Dto;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IPokemonRepository
	{
		ICollection<Pokemon> GetPokemons();
		Pokemon GetPokemon(int id);
		Pokemon GetPokemon(string name);
		ICollection<Review> GetPokemonReview(int pokeId);
		bool PokemonExists(int pokeId);
		bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
		bool UpdatePokemon(Pokemon pokemon);
		bool PokemonCategoryUpdate(PokemonCategory pokemonCategory);
		bool DeletePokemon(Pokemon pokemon);
		bool Save();
	}
}
