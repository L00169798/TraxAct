using Moq;
using TraxAct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.ViewModels;
using Xunit;

namespace TraxActUnitTest.Tests
{
	public class TimetableViewModelTests
	{
		[Fact]
		public async Task LoadEventsFromDatabase_Should_Load_Events_From_ViewModel()
		{
			var userId = "user123";
			var expectedEvents = GetTestEvents();

			var dbContextMock = new Mock<IDbContext>();
			dbContextMock.Setup(db => db.GetEventsByUserId(userId)).ReturnsAsync(expectedEvents);

			var userServiceMock = new Mock<IUserService>();
			userServiceMock.Setup(u => u.GetCurrentUserUid()).Returns(userId);
			Console.WriteLine($"UserId = {userId}");

			var viewModel = new TimetableViewModel(userServiceMock.Object);

			Console.WriteLine($"Expected Events Count before database call: {expectedEvents.Count}");

			await viewModel.LoadEventsFromDatabaseAsync();

			Console.WriteLine($"Actual Events Count: {viewModel.Events?.Count}");

			Assert.NotNull(viewModel.Events);
		}


		private List<Event> GetTestEvents()
		{
			return new List<Event>
			{
				new Event { EventId = 1, UserId="user123", Title="workout1", ExerciseType = "Swimming", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1)},
				new Event { EventId = 2, UserId="user123", Title="workout2", ExerciseType = "Pilates", StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(1).AddHours(2)}
			};
		}
	}
}
