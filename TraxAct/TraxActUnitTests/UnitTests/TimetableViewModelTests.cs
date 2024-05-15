using Microsoft.Maui.Graphics;
using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxActUnitTests.UnitTests
{
	public class TimetableViewModelTests
	{
		[Fact]
		public async Task LoadEventsFromDatabase_NoEvents_ReturnsEmptyEventsCollection()
		{
			// Arrange
			var userService = new UserService();
			var viewModel = new TimetableViewModel(userService);
			// Act
			await viewModel.LoadEventsFromDatabase();

			// Assert
			Assert.Empty(viewModel.Events);
		}

		[Fact]
		public void GetCategoryColor_UnknownSubject_ReturnsDefaultColor()
		{
			// Arrange
			var unknownSubject = "Unknown";

			// Act
			var result = TimetableViewModel.GetCategoryColor(unknownSubject);

			// Assert
			Assert.Equal(Colors.MistyRose, result);
		}

		[Fact]
		public void GetCategoryColor_KnownSubject_ReturnsCorrectColor()
		{
			// Arrange
			var knownSubject = "Walking";

			// Act
			var result = TimetableViewModel.GetCategoryColor(knownSubject);

			// Assert
			Assert.Equal(Colors.LightSeaGreen, result);
		}
	}

}

