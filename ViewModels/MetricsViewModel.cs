using Microsoft.EntityFrameworkCore;
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
				OnPropertyChanged();
			}
		}

		private ObservableCollection<ExerciseByDayOfWeek> _totalExerciseByDayOfWeek;
		public ObservableCollection<ExerciseByDayOfWeek> TotalExerciseByDayOfWeek
		{
			get { return _totalExerciseByDayOfWeek; }
			set
			{
				_totalExerciseByDayOfWeek = value;
				OnPropertyChanged();
			}
		}

		public MetricsViewModel()
		{
			Console.WriteLine("MetricsViewModel instantiated.");
			try
			{
				_dbContext = new MyDbContext();
				_userService = new UserService();
				LoadData();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error initializing MetricsViewModel: {ex.Message}");
			}
		}

		private async void LoadData()
		{
			try
			{
				Console.WriteLine("LoadData method called.");
				string currentUserUid = UserService.Instance.GetCurrentUserUid();
				if (string.IsNullOrEmpty(currentUserUid))
				{
					Console.WriteLine("Current user ID is null or empty. Data loading aborted.");
					return;
				}

				Console.WriteLine($"Loading events for user ID: {currentUserUid}");

				var events = await _dbContext.GetEventsByUserId(currentUserUid);

				if (events == null || events.Count == 0)
				{
					Console.WriteLine("No events found for the specified user.");
					return;
				}

				ExerciseHours = ConvertToExerciseHours(events);
				LogExerciseHours(ExerciseHours);

				// Calculate and populate TotalExerciseByDayOfWeek
				CalculateTotalExerciseByDayOfWeek(events);
				LogTotalExerciseByDayOfWeek();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading events: {ex.Message}");
			}
		}

		private Dictionary<string, double> ConvertToExerciseHours(List<Event> events)
		{
			Dictionary<string, double> exerciseHours = new Dictionary<string, double>();

			try
			{
				if (events == null)
				{
					Console.WriteLine("The events list is null or empty.");
					return exerciseHours;
				}

				foreach (var ev in events)
				{
					if (ev == null)
					{
						Console.WriteLine("Encountered a null event object.");
						continue;
					}

					TimeSpan duration = ev.EndTime - ev.StartTime;
					double durationHours = duration.TotalHours;

					if (exerciseHours.ContainsKey(ev.ExerciseType))
					{
						exerciseHours[ev.ExerciseType] += durationHours;
					}
					else
					{
						exerciseHours[ev.ExerciseType] = durationHours;
					}
				}

				Console.WriteLine($"Converted {events.Count} events to ExerciseHours.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error converting events to ExerciseHours: {ex.Message}");
			}

			return exerciseHours;
		}

		private void LogExerciseHours(Dictionary<string, double> exerciseHours)
		{
			if (exerciseHours == null || exerciseHours.Count == 0)
			{
				Console.WriteLine("ExerciseHours dictionary is null or empty.");
				return;
			}

			Console.WriteLine("ExerciseHours Summary:");

			foreach (var kvp in exerciseHours)
			{
				Console.WriteLine($"Exercise: {kvp.Key}, Total Hours: {kvp.Value}");
			}
		}

		private void CalculateTotalExerciseByDayOfWeek(List<Event> events)
		{
			var groupedByDayOfWeek = events.GroupBy(ev => ev.StartTime.DayOfWeek);

			TotalExerciseByDayOfWeek = new ObservableCollection<ExerciseByDayOfWeek>();

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

		private void LogTotalExerciseByDayOfWeek()
		{
			if (TotalExerciseByDayOfWeek == null || TotalExerciseByDayOfWeek.Count == 0)
			{
				Console.WriteLine("TotalExerciseByDayOfWeek collection is null or empty.");
				return;
			}

			Console.WriteLine("Total Exercise Count by Day of Week:");

			foreach (var item in TotalExerciseByDayOfWeek)
			{
				Console.WriteLine($"{item.DayOfWeek}: {item.ExerciseCount} hours");
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public class ExerciseByDayOfWeek
		{
			public DayOfWeek DayOfWeek { get; set; }
			public double ExerciseCount { get; set; }
		}
	}
}
