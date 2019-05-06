namespace Kritikos.TitanicModel
{
	using Microsoft.ML.Data;

	public class OutputModel
	{
		[ColumnName("PredictedLabel")]
		public bool Survived { get; set; }

		[ColumnName("Probability")]
		public float Probability { get; set; }
	}
}
