﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class AnalysisViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		private readonly IUserService _userService;
		private IDbContext _dbContext;

		public string UserId { get; }

		private Dictionary<string, double> _exerciseHours;
		public Dictionary<string, double> ExerciseHours
		{
			get { return _exerciseHours; }
			set
			{
				_exerciseHours = value;
				OnPropertyChanged();
			}
		}

		//public IUserService _userService
		//{
		//	get { return _userService; }
		//	set
		//	{
		//		if (_userService != value)
		//		{
		//			_userService = value;
		//			OnPropertyChanged();
		//		}
		//	}
		//}


		public IDbContext DbContext
		{
			get { return _dbContext; }
			set
			{
				if (_dbContext != value)
				{
					_dbContext = value;
					OnPropertyChanged();
				}
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

		public AnalysisViewModel(IUserService userService)
		{
			_dbContext = new MyDbContext();
			_userService = userService;

			StartDate = DateTime.Today.AddDays(-7);
			EndDate = DateTime.Today;
			ApplyCommand = new Command(ExecuteApplyCommand);
		}

		public ICommand ApplyCommand { get; private set; }

		//private bool CanExecuteApplyCommand(object parameter)
		//{
		//	return StartDate <= EndDate;
		//}

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
				filteredEvents.Sort((ev1, ev2) => ev1.StartTime.CompareTo(ev2.StartTime));

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

			var groupedExerciseHours = events
				.Where(ev => !string.IsNullOrEmpty(ev.ExerciseType))
				.GroupBy(ev => ev.ExerciseType)
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