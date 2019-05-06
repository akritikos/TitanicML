namespace Kritikos.TitanicExecutable
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	using Kritikos.TitanicModel;
	using Microsoft.ML;
	using Microsoft.ML.Transforms;

	public static class Program
	{
		public static void Main()
		{
			Console.WriteLine("Hello World!");

			var mlContext = new MLContext();

			var textLoader = mlContext.Data.CreateTextLoader<Passenger>(separatorChar: ',', hasHeader: true);

			var assemblyPath = Assembly.GetExecutingAssembly().Location;

			var assetsPath = new DirectoryInfo(assemblyPath)?
				.Parent?.Parent?.Parent?.Parent?.Parent?
				.GetDirectories("assets")
				.FirstOrDefault();

			if (!(assetsPath?.Exists ?? false))
			{
				Console.WriteLine($"Expected datasets at {assetsPath.FullName} and found none!");
			}

			var trainData = textLoader
				.Load($@"{assetsPath.FullName}\titanic.train.csv");

			var validateData = textLoader
				.Load($@"{assetsPath.FullName}\titanic.validate.csv");

			var mlPipeline = mlContext.Transforms
				.DropColumns(
					nameof(Passenger.PassengerId),
					nameof(Passenger.Name),
					nameof(Passenger.Ticket),
					nameof(Passenger.Fare),
					nameof(Passenger.Cabin))
				.Append(mlContext.Transforms.ReplaceMissingValues(
					nameof(Passenger.Age),
					nameof(Passenger.Age),
					MissingValueReplacingEstimator.ColumnOptions.ReplacementMode.Mean))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding(
					nameof(Passenger.Gender),
					nameof(Passenger.Gender)))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding(
					nameof(Passenger.Embarked),
					nameof(Passenger.Embarked)))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding(
					nameof(Passenger.PassengerClass),
					nameof(Passenger.PassengerClass)))
				.Append(mlContext.Transforms.Concatenate(
					"Features",
					nameof(Passenger.PassengerClass),
					nameof(Passenger.Gender),
					nameof(Passenger.Age),
					nameof(Passenger.SiblingsOrSpouses),
					nameof(Passenger.ParentsOrChildren),
					nameof(Passenger.Embarked)))
				.Append(mlContext.BinaryClassification.Trainers.FastTree(
					nameof(Passenger.Survived)))
				.Fit(trainData);

			var stats = mlContext.BinaryClassification.EvaluateNonCalibrated(
				mlPipeline.Transform(trainData),
				nameof(OutputModel.Survived));

			Console.WriteLine("Training statistics:");
			Console.WriteLine($"\tAccuracy: {stats.Accuracy}");
			Console.WriteLine($"\tF1: {stats.F1Score}");

			var predictor = mlPipeline.CreatePredictionEngine<PredictedData, OutputModel>(mlContext);
			foreach (var row in validateData.Preview(2000).RowView)
			{
				var inputModel = new PredictedData
				{
					Embarked = row.Values[11].Value.ToString(),
					PassengerClass = (float)row.Values[2].Value,
					Gender = row.Values[4].Value.ToString(),
					Age = (float)row.Values[5].Value,
					ParentsOrChildren = (float)row.Values[7].Value,
					SiblingsOrSpouses = (float)row.Values[6].Value,
					Cabin = row.Values[10].Value.ToString(),
					Name = row.Values[3].Value.ToString(),
					Fare = (double)row.Values[9].Value,
					Ticket = row.Values[8].Value.ToString(),
					PassengerId = (int)row.Values[0].Value,
				};

				var prediction = predictor.Predict(inputModel);
				Console.WriteLine($"{inputModel.PassengerId}, {(prediction.Survived ? "1" : "0")}");
			}

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
