using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PokemonReviewApp.Interfaces
{
	public interface IGlobalRepository
	{
		bool GlobalSave();
		void StopTrackingEntity(object entityEntry);
	}
}
