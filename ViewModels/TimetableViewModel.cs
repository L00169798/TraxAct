using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
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

		private DataTemplate _customAppointmentViewTemplate;
		public DataTemplate CustomAppointmentViewTemplate
		{
			get { return _customAppointmentViewTemplate; }
			private set
			{
				_customAppointmentViewTemplate = value;
				OnPropertyChanged(nameof(CustomAppointmentViewTemplate));
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


		public async void LoadEventsFromDatabase()
		{
			Debug.WriteLine("Loading events for User ID: " + UserId);
			if (string.IsNullOrEmpty(UserId))
			{
				Debug.WriteLine("UserId is null or empty. Cannot load events.");
				return;
			}

			try
			{

				var events = await _dbContext.GetEventsByUserId(UserId);
				if (events == null)
				{
					Debug.WriteLine("No events found for the specified UserId.");
					return;
				}

				Debug.WriteLine($"Loaded {events.Count} events from the database for user ID: {UserId}");

				foreach (var ev in events)
				{
					var schedulerAppointment = new SchedulerAppointment
					{
						Subject = ev.ExerciseType, 
						StartTime = ev.StartTime,
						EndTime = ev.EndTime,
						Id = ev.EventId
					};
					Events.Add(schedulerAppointment);

					Debug.WriteLine($"Event ID: {schedulerAppointment.Id}");
					Debug.WriteLine($"Event Subject: {schedulerAppointment.Subject}");
					Debug.WriteLine($"StartTime: {schedulerAppointment.StartTime}");
					Debug.WriteLine($"EndTime: {schedulerAppointment.EndTime}");
					Debug.WriteLine($"ExerciseType: {schedulerAppointment.Location}");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading events: {ex.Message}");
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			Debug.WriteLine($"PropertyChanged event invoked for property: {propertyName}");
		}
	}
}
