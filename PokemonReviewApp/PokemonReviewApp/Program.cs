using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
			builder.Services.AddScoped<IGlobalRepository, GlobalRepository>();
			builder.Services.AddScoped<IPokemonCategoryRepository, PokemonCategoryRepository>();
			builder.Services.AddScoped<IPokemonOwnerRepository, PokemonOwnerRepository>();
			builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
			builder.Services.AddScoped<ICountryRepository, CountryRepository>();
			builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
			builder.Services.AddDbContext<DataContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}