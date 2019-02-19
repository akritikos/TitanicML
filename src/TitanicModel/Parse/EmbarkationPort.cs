namespace TitanicModel.Parse
{
	using System.ComponentModel;

	public enum EmbarkationPort
	{
		Undefined = 0,
		[Description("Cherbourg")]
		C,
		[Description("Queenstown")]
		Q,
		[Description("Southampton")]
		S,
	}
}
