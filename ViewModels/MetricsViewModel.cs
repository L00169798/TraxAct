using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
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
		public ObservableCollection<ExerciseByDay> TotalExerciseByDay { get; private set; }

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
				string userId = UserService.Instance.GetCurrentUserUid();

				if (string.IsNullOrEmpty(userId))
				{
					Debug.WriteLine("User ID is null or empty. Skipping filter execution.");
					return;
				}

				Debug.WriteLine($"User ID: {userId}");

				var filteredEvents = await _dbContext.GetEventsByTimeAsync(StartDate, EndDate, userId);

				FilteredExerciseHours = ConvertToExerciseHours(filteredEvents);
				CalculateTotalExerciseByDay(filteredEvents);

				OnPropertyChanged(nameof(FilteredExerciseHours));
				OnPropertyChanged(nameof(TotalExerciseByDay));
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

		private void CalculateTotalExerciseByDay(List<Event> events)
		{
			TotalExerciseByDay = new ObservableCollection<ExerciseByDay>();

			if (events == null || !events.Any())
			{
				Console.WriteLine("No events found in the specified date range.");
				OnPropertyChanged(nameof(TotalExerciseByDay));
				return;
			}

			events = events.Where(ev => ev.StartTime.Date >= StartDate.Date && ev.EndTime.Date <= EndDate.Date).ToList();

		
			var groupedByDate = events.GroupBy(ev => ev.StartTime.Date);

			foreach (var group in groupedByDate)
			{
				double totalHours = group.Sum(ev => (ev.EndTime - ev.StartTime).TotalHours);

				var exerciseDay = new ExerciseByDay
				{
					Date = group.Key,
					TotalExerciseHours = totalHours
				};

				TotalExerciseByDay.Add(exerciseDay);

				Console.WriteLine($"Exercise on {exerciseDay.Date.ToShortDateString()}: {exerciseDay.TotalExerciseHours} hours");
			}

			OnPropertyChanged(nameof(TotalExerciseByDay));
		}



		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
