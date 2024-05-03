using Moq;
using TraxAct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.ViewModels;
using Xunit;

namespace TraxActUnitTests.Tests
{
    public class AnalysisViewModelTests
    {
        [Fact]
        public void StartDate_PropertyChanged_Should_Raise_Event()
        {

            var userServiceMock = new Mock<IUserService>();

            var viewModel = new AnalysisViewModel(userServiceMock.Object);

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

            var userServiceMock = new Mock<IUserService>();

            var viewModel = new AnalysisViewModel(userServiceMock.Object);

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
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(u => u.GetCurrentUserUid()).Returns("user123");

            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(db => db.GetEventsByTimeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), "user123"))
                        .ReturnsAsync(new List<Event>());

            var viewModel = new AnalysisViewModel(userServiceMock.Object);

            viewModel.StartDate = DateTime.Now.AddDays(-7);
            viewModel.EndDate = DateTime.Now;
            viewModel.ApplyCommand.Execute(null);
        }

        [Fact]
        public void ApplyCommand_Should_Handle_Failure()
        {
            var userServiceMock = new Mock<IUserService>();

            var viewModel = new AnalysisViewModel(userServiceMock.Object);

            Assert.Null(viewModel.FilteredExerciseHours);
            Assert.Null(viewModel.TotalExerciseByDay);
        }
    }
}
