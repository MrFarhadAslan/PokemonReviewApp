using PokemonReviewApp.Dto;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface IOwnerRepository
	{
		bool OwnerExist(int id);
		Owner GetOwnerById(int ownerId);
		ICollection<Owner> GetOwners();
		ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
		ICollection<Pokemon> GetPokemonByOwner(int ownerId);
		bool Create(Owner owner);
		bool Update(Owner owner);
		bool Delete(Owner owner);
		bool Save();
	}
}
