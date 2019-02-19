namespace TitanicModel
{
	using System.Collections.Generic;
	using System.Linq;

	using FileHelpers;

	using TitanicModel.Parse;

	public static class LoadData
	{
		public static List<TitanicPassenger> PassengersFromFile(string filePath)
		{
			var engine = new FileHelperAsyncEngine<TitanicPassenger>();
			using (engine.BeginReadFile(filePath))
			{
				return engine.ToList();
			}
		}
	}
}
