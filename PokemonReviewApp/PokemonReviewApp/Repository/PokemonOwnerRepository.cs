using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class PokemonOwnerRepository : IPokemonOwnerRepository
	{
		private readonly DataContext _dataContext;

		public PokemonOwnerRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public bool Create(PokemonOwner owner)
		{
			_dataContext.PokemonOwners.Add(owner);
			return Save();
		}

		public bool Delete(PokemonOwner pokemonOwner)
		{
			_dataContext.PokemonOwners.Remove(pokemonOwner);
			return Save();
		}

		public PokemonOwner GetPokemonOwnerByIds(int pokiId, int ownerId)
		{
			return _dataContext.PokemonOwners.Where(x => x.PokemonId == pokiId && x.OwnerId == ownerId).FirstOrDefault();
		}

		public bool PokemonOwnerExist(int pokiId, int ownerId)
		{
			return _dataContext.PokemonOwners.Where(x => x.PokemonId == pokiId && x.OwnerId == ownerId).Any();
		}

		public bool Save()
		{
			var result = _dataContext.SaveChanges();
			return result > 0 ? true : false;
		}
	}
}
