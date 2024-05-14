using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	// ViewModel class for handling the display of events on the timetable
	public class TimetableViewModel : INotifyPropertyChanged
	{
		// Event handler for property changes
		public event PropertyChangedEventHandler PropertyChanged;

		// Readonly fields for UserService and DbContext
		private readonly UserService _userService;
		private readonly MyDbContext _dbContext;

		// Properties for ViewModel
		public string UserId { get; }
		public DateTime MinimumDateTime { get; set; }
		public bool ShowNavigationArrows { get; set; }
		public ICommand QueryAppointmentsCommand { get; set; }
		public bool ShowBusyIndicator { get; set; }

		/// <summary>
		/// Property for storing the selected date
		/// </summary>
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
					_ = LoadEventsFromDatabase();
				}
			}
		}

		/// <summary>
		/// Property for storing the background color of events
		/// </summary>
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

		/// <summary>
		/// Collection to hold events displayed on the timetable
		/// </summary>
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

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userService"></param>
		public TimetableViewModel(UserService userService)
		{
			_userService = userService;
			_dbContext = new MyDbContext();
			Events = new ObservableCollection<SchedulerAppointment>();
			UserId = _userService.GetCurrentUserUid();
			MinimumDateTime = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc); // Sets the minimum date
		}

		/// <summary>
		/// Method to get events filtered by date range from the database
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public async Task<List<Event>> GetEventsFilteredByDateRange(DateTime startDate, DateTime endDate)
		{
			List<Event> filteredEvents = new List<Event>();

				var events = await _dbContext.GetEventsByUserId(UserId);

				if (events == null || !events.Any())
				{
					return new List<Event>();
				}

			return filteredEvents;
		}

		/// <summary>
		/// Method to load events from the database
		/// </summary>
		/// <returns></returns>
		public async Task LoadEventsFromDatabase()
		{
			var events = await _dbContext.GetEventsByUserId(UserId);

			if (events == null || !events.Any())
			{
				return;
			}

			Events.Clear();
			foreach (var ev in events)
			{
				Color backgroundColor = GetCategoryColor(ev.ExerciseType); //Each exercies type has a designated colour

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
			}
		}

		/// <summary>
		/// Method to set event background color based on event type
		/// </summary>
		/// <param name="appointment"></param>
		public static void OnEventRendered(SchedulerAppointment appointment)
		{
			if (appointment != null)
			{
				appointment.Background = GetCategoryColor(appointment.Subject);
			}
		}

		/// <summary>
		/// Method to get category color based on event type
		/// </summary>
		/// <param name="subject"></param>
		/// <returns></returns>
		public static Color GetCategoryColor(string subject)
		{
			return subject switch
			{
				"Walking" => Colors.LightSeaGreen,
				"Swimming" => Colors.LightCoral,
				"Running" => Colors.LightSteelBlue,
				"Cycling" => Colors.BurlyWood,
				"Yoga" => Colors.LightGreen,
				"Pilates" => Colors.LightBlue,
				"Strength" => Colors.SandyBrown,
				"HIIT" => Colors.Aquamarine,
				"Circuit" => Colors.Beige,
				"Other" => Colors.AliceBlue,
				_ => Colors.MistyRose
			};
		}

		/// <summary>
		/// Method to invoke property changed event
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
