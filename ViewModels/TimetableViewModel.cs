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

namespace TraxAct.ViewModels
{
	public class TimetableViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _userId;
		public string UserId
		{
			get { return _userId; }
			set
			{
				if (_userId != value)
				{
					_userId = value;
					OnPropertyChanged(nameof(UserId));
					LoadEventsFromDatabase();
				}
			}
		}

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

		private readonly MyDbContext _dbContext;

		public TimetableViewModel(string userId)
		{
			_dbContext = new MyDbContext();
			Events = new ObservableCollection<SchedulerAppointment>();
			UserId = userId; 
		}

		public async void LoadEventsFromDatabase()
		{
			Debug.WriteLine("Load Events executed..");
			if (string.IsNullOrEmpty(UserId))
			{
				Debug.WriteLine("UserId is null or empty. Cannot load events.");
				return;
			}

			try
			{
				Events.Clear();

				DateTime startOfWeek = SelectedDate;
				DateTime endOfWeek = startOfWeek.AddDays(7);

				var events = await _dbContext.GetEventsByUserId(UserId);
				if (events == null)
				{
					Debug.WriteLine("No events found for the specified UserId.");
					return;
				}

				Debug.WriteLine($"Loaded {events.Count} events from the database for user ID: {UserId}");

				var filteredEvents = events
					.Where(e => (e.StartTime >= startOfWeek && e.StartTime < endOfWeek) ||
								(e.EndTime > startOfWeek && e.EndTime <= endOfWeek) ||
								(e.StartTime < startOfWeek && e.EndTime > endOfWeek))
					.OrderBy(e => e.StartTime)
					.ToList();

				Debug.WriteLine($"Filtered {filteredEvents.Count} events for date: {SelectedDate}");

				foreach (var ev in filteredEvents)
				{
					var schedulerAppointment = new SchedulerAppointment
					{
						Subject = ev.Subject,
						StartTime = ev.StartTime,
						EndTime = ev.EndTime,
						Id = ev.EventId
					};
					Events.Add(schedulerAppointment);

					Debug.WriteLine($"Event ID: {schedulerAppointment.Id}");
					Debug.WriteLine($"Event Subject: {schedulerAppointment.Subject}");
					Debug.WriteLine($"StartTime: {schedulerAppointment.StartTime}");
					Debug.WriteLine($"EndTime: {schedulerAppointment.EndTime}");
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
