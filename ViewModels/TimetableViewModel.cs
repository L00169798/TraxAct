using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.Scheduler;
using System.Threading.Tasks;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
    public class TimetableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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


        public TimetableViewModel()
        {
            _dbContext = new MyDbContext();
            Events = new ObservableCollection<SchedulerAppointment>();
            LoadEventsFromDatabase();
            AddSampleEvent();

        }

        private void AddSampleEvent()
        {
            var sampleEvent = new SchedulerAppointment
            {
                Subject = "Sample Event",
                StartTime = DateTime.Today.AddHours(10),
                EndTime = DateTime.Today.AddHours(12)
            };

            Events.Add(sampleEvent);
        }

        public async Task LoadEventsFromDatabase()
        {
            try
            {
                Events.Clear();

                DateTime startOfWeek = SelectedDate;
                DateTime endOfWeek = startOfWeek.AddDays(7);

                var events = await _dbContext.GetEvents();
                Debug.WriteLine($"Loaded {events.Count} events from the database.");

                var filteredEvents = events
            .Where(e => (e.StartTime >= startOfWeek && e.StartTime < endOfWeek) ||
                        (e.EndTime > startOfWeek && e.EndTime <= endOfWeek) ||
                        (e.StartTime < startOfWeek && e.EndTime > endOfWeek))
            .OrderBy(e => e.StartTime);


                Debug.WriteLine($"Filtered {filteredEvents.Count()} events for date: {SelectedDate}");

                Debug.WriteLine("Filtered Events:");
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
