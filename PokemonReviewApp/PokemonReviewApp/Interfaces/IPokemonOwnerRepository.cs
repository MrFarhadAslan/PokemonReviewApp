using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IPokemonOwnerRepository
	{
		bool PokemonOwnerExist(int pokiId, int ownerId);
		PokemonOwner GetPokemonOwnerByIds(int pokiId, int ownerId);
		bool Create(PokemonOwner owner);
		bool Delete(PokemonOwner pokemonOwner);
		bool Save();
	}
}
