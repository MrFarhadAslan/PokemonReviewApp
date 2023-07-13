namespace PokemonReviewApp.Model
{
	public class Pokemon
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirdDate { get; set; }
		public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

    }
}
