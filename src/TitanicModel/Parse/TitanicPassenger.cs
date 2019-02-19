namespace TitanicModel.Parse
{
	using FileHelpers;

	[DelimitedRecord(",")]
	[IgnoreFirst]
	public class TitanicPassenger
	{
		public int Id { get; set; }

		public bool Survived { get; set; }

		public PassengerClass Class { get; set; }

		[FieldQuoted]
		public string Name { get; set; }

		public Gender Gender { get; set; }

		public double? Age { get; set; }

		public int SiblingsSpouses { get; set; }

		public int ParentsChildren { get; set; }

		public string TicketNumber { get; set; }

		public double TicketFare { get; set; }

		public string Cabin { get; set; }

		public EmbarkationPort? Embarked { get; set; }
	}
}
