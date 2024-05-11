using Moq;
using TraxAct.Models;
using TraxAct.ViewModels;
using TraxAct.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TraxActUnitTests.UnitTests
{
	public class AnalysisViewModelTests
	{
		[Fact]
		public void ConvertToExerciseHours_NullEvents_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			List<Event>? events = null;

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
			Assert.Contains(result, kvp => kvp.Key == "Running" && Math.Abs(kvp.Value - 3) < 0.0001);
			Assert.Contains(result, kvp => kvp.Key == "Cycling" && Math.Abs(kvp.Value - 2) < 0.0001);
		}

		[Fact]
		public void CalculateTotalExerciseByDay_NullEvents_ReturnsEmptyCollection()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			List<Event>? events = null;

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
		public void CalculateTotalExerciseByDay_EventsInRange_ReturnsCorrectExerciseHours()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();
			viewModel.StartDate = DateTime.Today;
			viewModel.EndDate = DateTime.Today.AddDays(1);

			var events = new List<Event>
	{
		new Event { ExerciseType = "Swimming", StartTime = DateTime.Today.AddHours(8), EndTime = DateTime.Today.AddHours(10) },
		new Event { ExerciseType = "Running", StartTime = DateTime.Today.AddHours(12), EndTime = DateTime.Today.AddHours(13) },
		new Event { ExerciseType = "Walking", StartTime = DateTime.Today.AddDays(1).AddHours(10), EndTime = DateTime.Today.AddDays(1).AddHours(12) }
	};

			// Act
			viewModel.CalculateTotalExerciseByDay(events);

			// Assert
			Assert.Equal(2, viewModel.TotalExerciseByDay.Count);

			var expectedHours = new Dictionary<DateTime, double>
	{
		{ DateTime.Today, 3 },
		{ DateTime.Today.AddDays(1), 2 }
	};

			foreach (var exerciseDay in viewModel.TotalExerciseByDay)
			{
				Assert.True(expectedHours.ContainsKey(exerciseDay.Date));
				Assert.Equal(expectedHours[exerciseDay.Date], exerciseDay.TotalExerciseHours);
			}
		}

	}
}
