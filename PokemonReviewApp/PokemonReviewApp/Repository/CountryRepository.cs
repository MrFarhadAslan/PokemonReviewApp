using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository
{
	public class CountryRepository:ICountryRepository
	{
		private readonly DataContext _dataContext;

		public CountryRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}

		public bool Create(Country country)
		{
			_dataContext.Countries.Add(country);
			return Save();
		}

		public bool Delete(Country country)
		{
			_dataContext.Countries.Remove(country);
			return Save();
		}

		public bool ExistCountry(int id)
		{
			return _dataContext.Countries.Any(c => c.Id == id);
		}

		public ICollection<Country> GetCountries()
		{
			return _dataContext.Countries.ToList();
		}

		public Country GetCountry(int id)
		{
			return _dataContext.Countries.FirstOrDefault(x => x.Id == id);
		}

		public Country GetCountryByOwner(int ownerId)
		{
			//return _dataContext.Countries.Where(x=>x.Owners.Any(x=>x.Id == ownerId)).FirstOrDefault();
			return _dataContext.Owners.Where(x => x.Id == ownerId).Select(x => x.Country).FirstOrDefault();
		}

		public ICollection<Owner> GetOwnersFromACountry(int countryId)
		{
			return _dataContext.Owners.Where(c => c.Country.Id == countryId).ToList();
		}

		public bool Save()
		{
			return _dataContext.SaveChanges() > 0 ? true : false;
		}

		public bool Update(Country country)
		{
			_dataContext.Countries.Update(country);
			return Save();
		}
	}
}
