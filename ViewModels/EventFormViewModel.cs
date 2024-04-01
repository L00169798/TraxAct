using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.Services;
using System.Collections.ObjectModel;

namespace TraxAct.ViewModels
{
    public class EventFormViewModel : INotifyPropertyChanged
    {
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

        public List<string> ExerciseTypes { get; } = new List<string>
        {
            "Walking",
            "Running",
            "Cycling",
            "Swimming",
            "Yoga",
            "Pilates",
            "StrengthTraining",
            "HIIT",
            "CircuitTraining",
            "Other"
        };

        public Command SaveCommand { get; }

        public EventFormViewModel()
        {
            SaveCommand = new Command(ExecuteSaveCommand);
            IsDistanceVisible = true;
            _dbContext = new MyDbContext();

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        public EventFormViewModel(Event SelectedEvent)
        {
        }

        private MyDbContext _dbContext;

        public bool IsValid
        {
            get { return Validate(); }
        }

        private bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Subject);
        }

        private async void ExecuteSaveCommand()
        {
            try
            {
                if (!IsValid)
                {
                    Console.WriteLine("Validation failed. Please check your inputs.");
                    return;
                }


                DateTime startDateTime = StartDate.Date.Add(StartTime);


                DateTime endDateTime = EndDate.Date.Add(EndTime);


                Event newEvent = new Event
                {
                    Subject = Subject,
                    ExerciseType = SelectedExerciseType,
                    StartTime = startDateTime,
                    EndTime = endDateTime,
                    Distance = Distance,
                    Reps = Reps,
                    Sets = Sets
                };

                Console.WriteLine("Saving event...");

                bool result = await _dbContext.Create(newEvent);

                if (result)
                {
                    EventList.Add(newEvent);
                    Console.WriteLine("Event saved successfully.");

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    Console.WriteLine("Failed to save event.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving event: {ex.Message}");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
