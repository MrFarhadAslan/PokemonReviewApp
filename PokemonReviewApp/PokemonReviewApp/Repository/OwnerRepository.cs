using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class OwnerRepository : IOwnerRepository
	{
		private readonly DataContext _dataContext;

		public OwnerRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

		public bool Create(Owner owner)
		{
			_dataContext.Owners.Add(owner);
			return Save();
		}

		public bool Delete(Owner owner)
		{
			_dataContext.Owners.Remove(owner);
			return Save();
		}

		public Owner GetOwnerById(int ownerId)
		{
			return _dataContext.Owners.FirstOrDefault(x => x.Id == ownerId);
		}

		public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
		{
			return _dataContext.PokemonOwners.Where(x=>x.PokemonId ==  pokeId).Select(o=>o.Owner).ToList();
		}

		public ICollection<Owner> GetOwners()
		{
			return _dataContext.Owners.ToList();
		}

		public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
		{
			return _dataContext.PokemonOwners.Where(x => x.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
		}

		public bool OwnerExist(int id)
		{
			return _dataContext.Owners.Any(x => x.Id == id);
		}

		public bool Save()
		{
			return _dataContext.SaveChanges() > 0 ? true : false;
		}

		public bool Update(Owner owner)
		{
			_dataContext.Owners.Update(owner);
			return Save();
		}
	}
}
