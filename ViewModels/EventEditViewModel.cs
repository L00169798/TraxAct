using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;
using Microsoft.Maui.Controls;
using System.Globalization;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
    public class EventEditViewModel : INotifyPropertyChanged
    {
        private readonly MyDbContext _dbContext;
        private Event _selectedEvent;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _selectedExerciseType;
        public string SelectedExerciseType
        {
            get { return _selectedExerciseType; }
            set
            {
                if (_selectedExerciseType != value)
                {
                    _selectedExerciseType = value;
                    OnPropertyChanged(nameof(SelectedExerciseType));

                    UpdateDistanceVisibility();
                    UpdateRepsVisibility();
                    UpdateSetsVisibility();

                }
            }
        }

        private void UpdateDistanceVisibility()
        {
            IsDistanceVisible = SelectedExerciseType == "Running";
        }

        private void UpdateRepsVisibility()
        {
            IsRepsVisible = SelectedExerciseType == "StrengthTraining";
        }

        private void UpdateSetsVisibility()
        {
            IsSetsVisible = SelectedExerciseType == "StrengthTraining";
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
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
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _distance;
        public double Distance
        {
            get { return _distance; }
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isDistanceVisible;
        public bool IsDistanceVisible
        {
            get { return _isDistanceVisible; }
            set
            {
                if (_isDistanceVisible != value)
                {
                    _isDistanceVisible = value;
                    OnPropertyChanged(nameof(IsDistanceVisible));
                }
            }
        }

        private int _reps;
        public int Reps
        {
            get { return _reps; }
            set
            {
                if (_reps != value)
                {
                    _reps = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _sets;
        public int Sets
        {
            get { return _sets; }
            set
            {
                if (_sets != value)
                {
                    _sets = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isSetsVisible;
        public bool IsSetsVisible
        {
            get { return _isSetsVisible; }
            set
            {
                if (_isSetsVisible != value)
                {
                    _isSetsVisible = value;
                    OnPropertyChanged(nameof(IsSetsVisible));
                }
            }
        }

        private bool _isRepsVisible;
        public bool IsRepsVisible
        {
            get { return _isRepsVisible; }
            set
            {
                if (_isRepsVisible != value)
                {
                    _isRepsVisible = value;
                    OnPropertyChanged(nameof(IsRepsVisible));
                }
            }
        }

        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    OnPropertyChanged();

                    if (_selectedEvent != null)
                    {
                        StartDate = _selectedEvent.StartTime;
                        EndDate = _selectedEvent.EndTime;
                    }
                }
            }
        }

        private DateTime ParseDateTime(string dateTimeString)
        {
            if (DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new ArgumentException("Invalid date format");
            }
        }


        public ICommand SaveCommand { get; }

        public EventEditViewModel(MyDbContext dbContext, Event selectedEvent)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            SelectedEvent = selectedEvent ?? throw new ArgumentNullException(nameof(selectedEvent));

            Subject = selectedEvent.Subject;
            SelectedExerciseType = selectedEvent.ExerciseType;
            StartDate = selectedEvent.StartTime;
            EndDate = selectedEvent.EndTime;
            Distance = selectedEvent.Distance;
            Reps = selectedEvent.Reps;
            Sets = selectedEvent.Sets;

            SaveCommand = new Command(SaveEvent);
        }
        private async void SaveEvent()
        {
            try
            {
                
                DateTime startDateTime = StartDate.Date.Add(StartTime);
                DateTime endDateTime = EndDate.Date.Add(EndTime);

                SelectedEvent.Subject = Subject;
                SelectedEvent.ExerciseType = SelectedExerciseType;
                SelectedEvent.StartTime = startDateTime;
                SelectedEvent.EndTime = endDateTime;
                SelectedEvent.Distance = Distance;
                SelectedEvent.Reps = Reps;
                SelectedEvent.Sets = Sets;

                _dbContext.Update(SelectedEvent);

                await Application.Current.MainPage.DisplayAlert("Success", "Event updated successfully.", "OK");

                await Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
