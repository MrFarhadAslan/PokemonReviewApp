using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class PokemonRepository : IPokemonRepository
	{
		private readonly DataContext _dataContext;

		public PokemonRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

		public Pokemon GetPokemon(int id)
		{
			return _dataContext.Pokemons.FirstOrDefault(x => x.Id == id);
		}

		public Pokemon GetPokemon(string name)
		{
			return _dataContext.Pokemons.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
		}

		public ICollection<Review> GetPokemonReview(int pokeId)
		{
			return _dataContext.Reviews.Where(x => x.PokemonId == pokeId).ToList();
		}

		public ICollection<Pokemon> GetPokemons()
		{
			return _dataContext.Pokemons.ToList();
		}

		public bool PokemonExists(int pokeId)
		{
			return _dataContext.Pokemons.Any(x => x.Id == pokeId);
		}

		public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
		{
			var owner = _dataContext.Owners.FirstOrDefault(x => x.Id == ownerId);
			var category = _dataContext.Categories.FirstOrDefault(x => x.Id == categoryId);

			PokemonCategory pokemonCategory = new PokemonCategory
			{
				Category = category,
				Pokemon = pokemon
			};

			PokemonOwner pokemonOwner = new PokemonOwner
			{
				Owner = owner,
				Pokemon = pokemon
			};

			_dataContext.PokemonCategories.Add(pokemonCategory);
			_dataContext.PokemonOwners.Add(pokemonOwner);
			_dataContext.Pokemons.Add(pokemon);

			return Save();
		}
		public bool UpdatePokemon(Pokemon pokemon)
		{
			_dataContext.Pokemons.Update(pokemon);
			return Save();
		}

		public bool DeletePokemon(Pokemon pokemon)
		{
			_dataContext.Pokemons.Remove(pokemon);
			return Save();
		}

		public bool Save()
		{
			var result = _dataContext.SaveChanges();
			return result > 0 ? true : false;
		}

		public bool PokemonCategoryUpdate(PokemonCategory pokemonCategory)
		{
			_dataContext.PokemonCategories.Update(pokemonCategory);
			return Save();
		}
	}
}
