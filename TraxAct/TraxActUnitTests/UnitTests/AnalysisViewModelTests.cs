using TraxAct.Models;
using TraxAct.ViewModels;

namespace TraxActUnitTests.UnitTests
{
	public class AnalysisViewModelTests
	{
		[Fact]
		public void ConvertToExerciseHours_NullEvents_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			List<Event> events = null;

			// Act
			var result = viewModel.ConvertToExerciseHours(events);

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void ConvertToExerciseHours_NoEventsInRange_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			var events = new List<Event>();

			// Act
			var result = viewModel.ConvertToExerciseHours(events);

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void ConvertToExerciseHours_EventsInRange_ReturnsGroupedCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			var events = new List<Event>
			{
				new Event { ExerciseType = "Running", StartTime = DateTime.Today.AddHours(8), EndTime = DateTime.Today.AddHours(10) },
				new Event { ExerciseType = "Cycling", StartTime = DateTime.Today.AddHours(10), EndTime = DateTime.Today.AddHours(12) },
				new Event { ExerciseType = "Running", StartTime = DateTime.Today.AddHours(14), EndTime = DateTime.Today.AddHours(15) }
			};

			// Act
			var result = viewModel.ConvertToExerciseHours(events);

			// Assert
			Assert.Equal(2, result.Count);
			Assert.Contains(result, kvp => kvp.Key == "Running" && kvp.Value == 3);
			Assert.Contains(result, kvp => kvp.Key == "Cycling" && kvp.Value == 2);
		}

		[Fact]
		public void CalculateTotalExerciseByDay_NullEvents_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			List<Event> events = null;

			// Act
			viewModel.CalculateTotalExerciseByDay(events);

			// Assert
			Assert.Empty(viewModel.TotalExerciseByDay);
		}

		[Fact]
		public void CalculateTotalExerciseByDay_NoEventsInRange_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			var events = new List<Event>();

			// Act
			viewModel.CalculateTotalExerciseByDay(events);

			// Assert
			Assert.Empty(viewModel.TotalExerciseByDay);
		}

		[Fact]
		public void CalculateTotalExerciseByDay_EventsInRange_ReturnsGroupedCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			viewModel.StartDate = DateTime.Today;
			viewModel.EndDate = DateTime.Today.AddDays(1);
			var events = new List<Event>
			{
				new Event { StartTime = DateTime.Today.AddHours(8), EndTime = DateTime.Today.AddHours(10) },
				new Event { StartTime = DateTime.Today.AddHours(10), EndTime = DateTime.Today.AddHours(12) },
				new Event { StartTime = DateTime.Today.AddDays(1).AddHours(14), EndTime = DateTime.Today.AddDays(1).AddHours(15) }
			};

			// Act
			viewModel.CalculateTotalExerciseByDay(events);

			// Assert
			Assert.Equal(2, viewModel.TotalExerciseByDay.Count);
			Assert.Contains(viewModel.TotalExerciseByDay, ed => ed.Date == DateTime.Today && ed.TotalExerciseHours == 4);
			Assert.Contains(viewModel.TotalExerciseByDay, ed => ed.Date == DateTime.Today.AddDays(1) && ed.TotalExerciseHours == 1);
		}
	}
}
