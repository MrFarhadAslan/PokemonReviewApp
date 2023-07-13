namespace PokemonReviewApp.Model
{
	public class Reviewer
	{
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
