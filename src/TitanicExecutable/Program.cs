namespace TitanicExecutable
{
	using System;
	using System.IO;
	using System.Linq;

	using TitanicModel;

	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			FileInfo training;
			FileInfo validate;

			try
			{
				var dir = new DirectoryInfo(Directory.GetCurrentDirectory())
					.Parent
					.Parent
					.Parent
					.Parent
					.Parent
					.GetDirectories("assets").First();
				training = dir.GetFiles("titanic.train.csv").First();
				validate = dir.GetFiles("titanic.validate.csv").First();
			}
			catch (Exception)
			{
				Console.WriteLine("Assets directory missing!");
				return;
			}

			foreach (var passenger in LoadData.PassengersFromFile(training.FullName))
			{
				Console.WriteLine($"{passenger.Name}");
			}

			Console.ReadLine();
		}
	}
}
