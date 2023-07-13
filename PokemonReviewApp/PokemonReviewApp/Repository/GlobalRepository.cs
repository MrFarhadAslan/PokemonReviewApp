using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
	public class GlobalRepository : IGlobalRepository
	{
		private readonly DataContext _dataContext;

		public GlobalRepository(DataContext dataContext)
        {
			_dataContext = dataContext;
		}
        public bool GlobalSave()
		{
			var result = _dataContext.SaveChanges();
			return result > 0 ? true: false;
		}

		public void StopTrackingEntity(object entityEntry)
		{
			var entry = _dataContext.Entry(entityEntry);

			if (entry.State != EntityState.Detached)
			{
				entry.State = EntityState.Detached;
			}
		}
	}
}
