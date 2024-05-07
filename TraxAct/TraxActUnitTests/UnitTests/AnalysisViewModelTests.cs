using Moq;
using TraxAct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.ViewModels;
using Xunit;
using Microsoft.Extensions.DependencyModel;

namespace TraxActUnitTests.Tests
{

	public class AnalysisViewModelTests
	{


		[Fact]
		public void StartDate_PropertyChanged_Should_Raise_Event()
		{

			var viewModel = new AnalysisViewModel();

			bool eventRaised = false;
			viewModel.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(AnalysisViewModel.StartDate))
					eventRaised = true;
			};

			viewModel.StartDate = DateTime.Now;

			Assert.True(eventRaised);
		}

		[Fact]
		public void EndDate_PropertyChanged_Should_Raise_Event()
		{


			var viewModel = new AnalysisViewModel();

			bool eventRaised = false;
			viewModel.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(AnalysisViewModel.EndDate))
					eventRaised = true;
			};

			viewModel.EndDate = DateTime.Now;

			Assert.True(eventRaised);
		}

		[Fact]
		public void ApplyCommand_Should_Execute_Successfully()
		{
			// Arrange
			var viewModel = new AnalysisViewModel();

			// Act
			viewModel.ApplyCommand.Execute(null);

			// Assert
			Assert.NotNull(viewModel.FilteredExerciseHours);
			Assert.NotNull(viewModel.TotalExerciseByDay);
		}


		[Fact]
		public void ApplyCommand_Should_Handle_Failure()
		{

			var viewModel = new AnalysisViewModel();

			Assert.Null(viewModel.FilteredExerciseHours);
			Assert.Null(viewModel.TotalExerciseByDay);
		}
	}
}
