using System.Collections.ObjectModel;
using System.ComponentModel;
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
		private readonly MyDbContext _dbContext;

		// Properties
		public string UserId { get; } 
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

			StartDate = DateTime.Today.AddDays(-7);
			EndDate = DateTime.Today;
		}

		// Apply Filter Command
		public ICommand ApplyCommand => new Command(ExecuteApplyCommand, CanExecuteApplyCommand);

		/// <summary>
		/// Retrieve dates for filtering
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanExecuteApplyCommand(object parameter)
		{
			return StartDate <= EndDate;
		}

		/// <summary>
		/// Execute Filter
		/// </summary>
		/// <param name="parameter"></param>
		private async void ExecuteApplyCommand(object parameter)
		{
				string userId = UserService.Instance.GetCurrentUserUid();

				if (string.IsNullOrEmpty(userId))
				{
					return;
				}

				var filteredEvents = await _dbContext.GetEventsByTimeAsync(StartDate, EndDate, userId);
				filteredEvents.Sort((ev1, ev2) => ev1.StartTime.CompareTo(ev2.StartTime)); // Sorted based on start times

				FilteredExerciseHours = ConvertToExerciseHours(filteredEvents);
				CalculateTotalExerciseByDay(filteredEvents);

			//Conversions required for analysis graphs
				OnPropertyChanged(nameof(FilteredExerciseHours));
				OnPropertyChanged(nameof(TotalExerciseByDay));
		}

		/// <summary>
		/// Converts events to exercise hours
		/// </summary>
		/// <param name="events"></param>
		/// <returns></returns>
		public ObservableCollection<KeyValuePair<string, double>> ConvertToExerciseHours(List<Event> events)
		{
			var exerciseHours = new ObservableCollection<KeyValuePair<string, double>>();

			if (events == null || !events.Any())
			{
				return exerciseHours;
			}

			//Groups exercises and calculates total time for each
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
				OnPropertyChanged(nameof(TotalExerciseByDay));
				return;
			}

			//Filters dates, groups by date and calculates total hours for each day
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
