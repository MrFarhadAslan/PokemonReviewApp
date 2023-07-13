using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces
{
	public interface ICountryRepository
	{
		bool ExistCountry(int id);
		Country GetCountry(int id);
		ICollection<Country> GetCountries();
		Country GetCountryByOwner(int ownerId);
		ICollection<Owner> GetOwnersFromACountry(int countryId);
		bool Create(Country country);
		bool Update(Country country);
		bool Delete(Country country);
		bool Save();
	}
}
