using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Maui.Core;
using TraxAct.Models;
using TraxAct.Services;
using Syncfusion.Maui.Scheduler;
using TraxAct.Views;
using System.Windows.Input;

namespace TraxAct.ViewModels
{
	public class TimetableViewModel : INotifyPropertyChanged

	{
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly UserService _userService;
		public string UserId { get; }
		public DateTime MinimumDateTime { get; set; }
		public bool ShowNavigationArrows { get; set; }
		public ICommand QueryAppointmentsCommand { get; set; }
		public bool ShowBusyIndicator { get; set; }

		private DateTime _selectedDate = DateTime.Today;
		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set
			{
				if (_selectedDate != value)
				{
					_selectedDate = value;
					OnPropertyChanged(nameof(SelectedDate));
					LoadEventsFromDatabase();
				}
			}
		}

		private Color _eventBackgroundColor;
		public Color EventBackgroundColor
		{
			get { return _eventBackgroundColor; }
			set
			{
				_eventBackgroundColor = value;
				OnPropertyChanged(nameof(EventBackgroundColor));
			}
		}

		private ObservableCollection<SchedulerAppointment> _events;
		public ObservableCollection<SchedulerAppointment> Events
		{
			get { return _events; }
			set
			{
				if (_events != value)
				{
					_events = value;
					OnPropertyChanged(nameof(Events));
				}
			}
		}

		public object userService { get; }

		private readonly MyDbContext _dbContext;

		public TimetableViewModel(UserService userService)
		{
			_userService = userService;
			_dbContext = new MyDbContext();
			Events = new ObservableCollection<SchedulerAppointment>();
			UserId = _userService.GetCurrentUserUid();
			MinimumDateTime = new DateTime(2024, 01, 01);
		}

	//	private Dictionary<string, Color> ExerciseTypeColors = new Dictionary<string, Color>
	//{
	//	{ "Walking", Colors.Green },
	//	{ "Swimming", Colors.Blue },
	//	{ "Cycling", Colors.Red },
	//	{ "Cycling", Colors.PaleTurquoise },
	//	{ "Yoga", Colors.Orange },
	//	{ "Pilates", Colors.LightCoral },
	//	{ "Strength", Colors.DarkCyan },
	//	{ "HIIT", Colors.Aqua },
	//	{ "Circuit", Colors.Salmon },
	//	{ "Other", Colors.BlueViolet }
	//};


		public async Task<List<Event>> GetEventsFilteredByDateRange(DateTime startDate, DateTime endDate)
		{
			List<Event> filteredEvents = new List<Event>();

			try
			{
				var events = await _dbContext.GetEventsByUserId(UserId);

				if (events == null || !events.Any())
				{
					Console.WriteLine("No events found in the database.");
					return filteredEvents;
				}


				Console.WriteLine($"Filtered {filteredEvents.Count} events based on date range.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error filtering events by date range: {ex.Message}");
			}

			return filteredEvents;
		}

		public async void LoadEventsFromDatabase()
		{
			try
			{
				var events = await _dbContext.GetEventsByUserId(UserId);

				if (events == null || !events.Any())
				{
					Debug.WriteLine("No events found for the specified UserId.");
					return;
				}

				Events.Clear();
				foreach (var ev in events)
				{
					Color backgroundColor = GetCategoryColor(ev.ExerciseType);

					var schedulerAppointment = new SchedulerAppointment
					{
						Subject = ev.ExerciseType,
						StartTime = ev.StartTime,
						EndTime = ev.EndTime,
						Id = ev.EventId,
						Background = backgroundColor,
						TextColor = Colors.Black
					};

					Events.Add(schedulerAppointment);

					Debug.WriteLine($"Event ID: {schedulerAppointment.Id}");
					Debug.WriteLine($"Event Subject: {schedulerAppointment.Subject}");
					Debug.WriteLine($"StartTime: {schedulerAppointment.StartTime}");
					Debug.WriteLine($"EndTime: {schedulerAppointment.EndTime}");
				}

				Debug.WriteLine($"Loaded {Events.Count} events from the database for user ID: {UserId}");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading events from the database: {ex.Message}");
			}
		}

		public void OnEventRendered(SchedulerAppointment appointment)
		{
			if (appointment != null)
			{
				appointment.Background = GetCategoryColor(appointment.Subject);
			}
		}

		private Color GetCategoryColor(string subject)
		{
			switch (subject)
			{
				case "Walking":
					return Colors.Green;
				case "Swimming":
					return Colors.LightCoral;
				case "Running":
					return Colors.Azure;
				case "Cycling":
					return Colors.BurlyWood;
				case "Yoga":
					return Colors.LightGreen;
				case "Pilates":
					return Colors.DarkViolet;
				case "Strength":
					return Colors.SandyBrown;
				case "HIIT":
					return Colors.Aquamarine;
				case "Circuit":
					return Colors.Beige;
				case "Other":
					return Colors.AliceBlue;
				default:
					return Colors.MistyRose;
			}
		}


		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			Debug.WriteLine($"PropertyChanged event invoked for property: {propertyName}");
		}
	}
}
