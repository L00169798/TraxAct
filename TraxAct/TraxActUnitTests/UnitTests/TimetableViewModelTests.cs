using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.ViewModels;
using Xunit;
using Syncfusion.Maui.Core;

namespace TraxActUnitTests.UnitTests
{
	public class TimetableViewModelTests
	{
		[Fact]
		public async Task GetEventsFilteredByDateRange_NoEvents_ReturnsEmptyList()
		{
			// Arrange
			var userService = new UserService();
			var viewModel = new TimetableViewModel(userService);
			var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc); 
			var endDate = new DateTime(2024, 1, 7, 23, 59, 59, DateTimeKind.Utc);

			// Act
			var result = await viewModel.GetEventsFilteredByDateRange(startDate, endDate);

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public async Task LoadEventsFromDatabase_NoEvents_ReturnsEmptyEventsCollection()
		{
			// Arrange
			var userService = new UserService();
			var viewModel = new TimetableViewModel(userService);

			// Act
			await Task.Run(() =>
			{
				viewModel.LoadEventsFromDatabase();
			});

			// Assert
			Assert.Empty(viewModel.Events);
		}

		[Fact]
		public void GetCategoryColor_UnknownSubject_ReturnsDefaultColor()
		{
			// Arrange
			var userService = new UserService();
			var viewModel = new TimetableViewModel(userService);
			var unknownSubject = "Unknown";

			// Act
			var result = viewModel.GetCategoryColor(unknownSubject);

			// Assert
			Assert.Equal(Colors.MistyRose, result);
		}

		[Fact]
		public void GetCategoryColor_KnownSubject_ReturnsCorrectColor()
		{
			// Arrange
			var userService = new UserService();
			var viewModel = new TimetableViewModel(userService);
			var knownSubject = "Walking";

			// Act
			var result = viewModel.GetCategoryColor(knownSubject);

			// Assert
			Assert.Equal(Colors.Green, result);
		}
	}
}
