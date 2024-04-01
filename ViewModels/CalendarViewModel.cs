using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private readonly MyDbContext _dbContext;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DetailsCommand { get; }

        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    LoadEvents(_currentDate);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Event> _eventList = new ObservableCollection<Event>();
        public ObservableCollection<Event> EventList
        {
            get { return _eventList; }
            set
            {
                _eventList = value;
                OnPropertyChanged();
            }
        }

        private bool _isNoEventsLabelVisible;
        public bool IsNoEventsLabelVisible
        {
            get { return _isNoEventsLabelVisible; }
            set
            {
                if (_isNoEventsLabelVisible != value)
                {
                    _isNoEventsLabelVisible = value;
                    OnPropertyChanged();
                }
            }
        }


        public CalendarViewModel(MyDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext), "MyDbContext instance cannot be null.");
            }

            _dbContext = dbContext;
            LoadEvents(DateTime.Today);
            DetailsCommand = new Command<Event>(OnDetailsCommand);
        }

        private async void OnDetailsCommand(object parameter)
        {
            if (parameter is Event eventId)
            {
                try
                {
                    Debug.WriteLine($"Executing DetailsCommand for event: {eventId}");

                    if (Shell.Current != null && Shell.Current.Navigation != null)
                    {
                        await Shell.Current.Navigation.PushAsync(new EventDetailsPage(eventId));
                        Debug.WriteLine("DetailsCommand execution completed successfully.");
                    }
                    else
                    {
                        Debug.WriteLine("Shell.Current or Shell.Current.Navigation is null. DetailsCommand execution failed.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error executing DetailsCommand: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("Event item is null, cannot execute DetailsCommand.");
            }
        }

        public async void LoadEvents(DateTime date)
        {
            try
            {
                EventList.Clear();

                var events = await _dbContext.GetEvents();
                foreach (var ev in events)
                {
                    Debug.WriteLine($"Event: {ev.Subject}, StartDateTime: {ev.StartTime}");
                }
                Debug.WriteLine($"Loaded {events.Count} events from the database.");

                var filteredEvents = events.Where(e => e.StartTime.Date == date.Date)
                                           .OrderBy(e => e.StartTime);

                Debug.WriteLine($"Filtered {filteredEvents.Count()} events for date: {date.Date}");

                foreach (var ev in filteredEvents)
                {
                    EventList.Add(ev);
                    Debug.WriteLine($"Added event '{ev.Subject}' to EventList.");
                }

                IsNoEventsLabelVisible = EventList.Count == 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading events: {ex.Message}");
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
