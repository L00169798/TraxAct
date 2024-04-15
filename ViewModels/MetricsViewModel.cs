using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace TraxAct.ViewModels
{
	public class MetricsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly UserService _userService;
		private readonly MyDbContext _dbContext;
		public string UserId { get; }

		private Dictionary<string, double> _exerciseHours;
		public Dictionary<string, double> ExerciseHours
		{
			get { return _exerciseHours; }
			set
			{
				_exerciseHours = value;
			}
		}

		private DateTime _startDate;
		public DateTime StartDate
		{
			get => _startDate;
			set
			{
				if (_startDate != value)
				{
					_startDate = value;
					OnPropertyChanged();
				}
			}
		}

		private DateTime _endDate;
		public DateTime EndDate
		{
			get => _endDate;
			set
			{
				if (_endDate != value)
				{
					_endDate = value;
					OnPropertyChanged();
				}
			}
		}

		public ObservableCollection<KeyValuePair<string, double>> FilteredExerciseHours { get; private set; }
		public ObservableCollection<ExerciseByDayOfWeek> TotalExerciseByDayOfWeek { get; private set; }

		public MetricsViewModel()
		{
			_dbContext = new MyDbContext();
			_userService = new UserService();

			StartDate = DateTime.Today.AddDays(-7);
			EndDate = DateTime.Today;
		}

		public ICommand ApplyCommand => new Command(ExecuteApplyCommand, CanExecuteApplyCommand);

		private bool CanExecuteApplyCommand(object parameter)
		{
			return StartDate <= EndDate;
		}

		private async void ExecuteApplyCommand(object parameter)
		{
			try
			{
				string userId = _userService.GetCurrentUserUid();

				var filteredEvents = await _dbContext.GetEventsByTimeAsync(StartDate, EndDate);

				FilteredExerciseHours = ConvertToExerciseHours(filteredEvents);
				CalculateTotalExerciseByDayOfWeek(filteredEvents);

				OnPropertyChanged(nameof(FilteredExerciseHours));
				OnPropertyChanged(nameof(TotalExerciseByDayOfWeek));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error applying filter: {ex.Message}");
			}
		}


		private ObservableCollection<KeyValuePair<string, double>> ConvertToExerciseHours(List<Event> events)
		{
			var exerciseHours = new ObservableCollection<KeyValuePair<string, double>>();

			if (events == null || !events.Any())
			{
				Console.WriteLine("No events found in the specified date range.");
				return exerciseHours;
			}

			var groupedExerciseHours = events.GroupBy(ev => ev.ExerciseType)
				.Select(group => new KeyValuePair<string, double>(group.Key, group.Sum(ev => (ev.EndTime - ev.StartTime).TotalHours)))
				.ToList();

			exerciseHours = new ObservableCollection<KeyValuePair<string, double>>(groupedExerciseHours);

			return exerciseHours;
		}

		private void CalculateTotalExerciseByDayOfWeek(List<Event> events)
		{
			TotalExerciseByDayOfWeek = new ObservableCollection<ExerciseByDayOfWeek>();

			if (events == null || !events.Any())
			{
				Console.WriteLine("No events found in the specified date range.");
				return;
			}

			var groupedByDayOfWeek = events.GroupBy(ev => ev.StartTime.DayOfWeek);

			foreach (var group in groupedByDayOfWeek)
			{
				var totalHours = group.Sum(ev => (ev.EndTime - ev.StartTime).TotalHours);

				TotalExerciseByDayOfWeek.Add(new ExerciseByDayOfWeek
				{
					DayOfWeek = group.Key,
					ExerciseCount = totalHours
				});
			}
		}

		public class ExerciseByDayOfWeek
		{
			public DayOfWeek DayOfWeek { get; set; }
			public double ExerciseCount { get; set; }
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
