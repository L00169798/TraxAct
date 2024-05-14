using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class AnalysisViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Fields
		private readonly UserService _userService;
		private readonly MyDbContext _dbContext;

		// Properties
		public string UserId { get; } // Consider removing if not used
		public Dictionary<string, double> ExerciseHours { get; set; }

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

		// Constructor
		public AnalysisViewModel()
		{
			_dbContext = new MyDbContext();
			_userService = new UserService();

			StartDate = DateTime.Today.AddDays(-7);
			EndDate = DateTime.Today;
		}

		// Command
		public ICommand ApplyCommand => new Command(ExecuteApplyCommand, CanExecuteApplyCommand);

		private bool CanExecuteApplyCommand(object parameter)
		{
			return StartDate <= EndDate;
		}

		private async void ExecuteApplyCommand(object parameter)
		{
			//try
			//{
				string userId = UserService.Instance.GetCurrentUserUid();

				if (string.IsNullOrEmpty(userId))
				{
					//Debug.WriteLine("User ID is null or empty. Skipping filter execution.");
					return;
				}

				//Debug.WriteLine($"User ID: {userId}");

				var filteredEvents = await _dbContext.GetEventsByTimeAsync(StartDate, EndDate, userId);
				filteredEvents.Sort((ev1, ev2) => ev1.StartTime.CompareTo(ev2.StartTime));

				FilteredExerciseHours = ConvertToExerciseHours(filteredEvents);
				CalculateTotalExerciseByDay(filteredEvents);

				OnPropertyChanged(nameof(FilteredExerciseHours));
				OnPropertyChanged(nameof(TotalExerciseByDay));
			//}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine($"Error applying filter: {ex.Message}");
			//}
		}

		// Converts events to exercise hours
		public ObservableCollection<KeyValuePair<string, double>> ConvertToExerciseHours(List<Event> events)
		{
			var exerciseHours = new ObservableCollection<KeyValuePair<string, double>>();

			if (events == null || !events.Any())
			{
				//Debug.WriteLine("No events found in the specified date range.");
				return exerciseHours;
			}

			var groupedExerciseHours = events
				.Where(ev => !string.IsNullOrEmpty(ev.ExerciseType))
				.GroupBy(ev => ev.ExerciseType)
				.Select(group => new KeyValuePair<string, double>(group.Key, group.Sum(ev => (ev.EndTime - ev.StartTime).TotalHours)))
				.ToList();

			exerciseHours = new ObservableCollection<KeyValuePair<string, double>>(groupedExerciseHours);

			return exerciseHours;
		}

		// Calculates total exercise hours by day
		public void CalculateTotalExerciseByDay(List<Event> events)
		{
			TotalExerciseByDay = new ObservableCollection<ExerciseByDay>();

			if (events == null || !events.Any())
			{
				//Debug.WriteLine("No events found in the specified date range.");
				OnPropertyChanged(nameof(TotalExerciseByDay));
				return;
			}

			events = events.Where(ev => ev.StartTime.Date >= StartDate.Date && ev.EndTime.Date <= EndDate.Date).ToList();

			var groupedByDate = events
				.Where(ev => ev.EndTime.Date >= StartDate.Date && ev.StartTime.Date <= EndDate.Date)
				.GroupBy(ev => ev.StartTime.Date);

			foreach (var group in groupedByDate)
			{
				double totalHours = group.Sum(ev => (ev.EndTime - ev.StartTime).TotalHours);

				var exerciseDay = new ExerciseByDay
				{
					Date = group.Key,
					TotalExerciseHours = totalHours
				};

				TotalExerciseByDay.Add(exerciseDay);

				//Debug.WriteLine($"Exercise on {exerciseDay.Date.ToShortDateString()}: {exerciseDay.TotalExerciseHours} hours");
			}

			OnPropertyChanged(nameof(TotalExerciseByDay));
		}

		// Property changed event
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
