namespace PokemonReviewApp.Helper
{
	public static class TypeConvert
	{
		public static TResult Convertion<T, TResult>(T model) where TResult : class, new()
		{
			TResult result = new TResult();

			typeof(T).GetProperties().ToList().ForEach(p =>
			{
				var property = typeof(TResult).GetProperty(p.Name);
				property?.SetValue(property, p.GetValue(model));

			});

			return result;
		}

		public static TResult Convertion<T, TResult>(T model, TResult exisObj)
		{
			typeof(T).GetProperties().ToList().ForEach(p =>
			{
				if (p.GetValue(model) is not null)
				{
					var property = typeof(TResult).GetProperty(p.Name);
					property?.SetValue(exisObj, p.GetValue(model));
				}

			});

			return exisObj;
		}
	}
}
