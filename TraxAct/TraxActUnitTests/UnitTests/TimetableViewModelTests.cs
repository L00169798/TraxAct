using TraxAct.ViewModels;

namespace TraxActUnitTests.UnitTests
{
	public class TimetableViewModelTests
	{
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

