using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Data
{
	public class DataContext:DbContext
	{
        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PokemonOwner>()
				.HasKey(x => new { x.OwnerId, x.PokemonId });

			modelBuilder.Entity<PokemonOwner>()
				.HasOne(x => x.Pokemon)
				.WithMany(x => x.PokemonOwners)
				.HasForeignKey(x => x.PokemonId);

			modelBuilder.Entity<PokemonOwner>()
				.HasOne(x => x.Owner)
				.WithMany(x => x.PokemonOwners)
				.HasForeignKey(x => x.OwnerId);

			modelBuilder.Entity<Country>()
				.HasMany(x => x.Owners)
				.WithOne(x => x.Country)
				.HasForeignKey(x => x.CountryId);

			modelBuilder.Entity<PokemonCategory>()
				.HasKey(x => new { x.CategoryId, x.PokemonId });

			modelBuilder.Entity<PokemonCategory>()
				.HasOne(x => x.Pokemon)
				.WithMany(x => x.PokemonCategories)
				.HasForeignKey(x => x.PokemonId);

			modelBuilder.Entity<PokemonCategory>()
				.HasOne(x => x.Category)
				.WithMany(x => x.PokemonCategories)
				.HasForeignKey(x => x.CategoryId);

			modelBuilder.Entity<Reviewer>()
				.HasMany(x => x.Reviews)
				.WithOne(x => x.Reviewer)
				.HasForeignKey(x => x.ReviewerId);
		}
	}
}
