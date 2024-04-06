using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TraxAct.Models;
using TraxAct.Views;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
    public class ExerciseViewModel : INotifyPropertyChanged
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



        private DateTime _startTime;
        public DateTime StartTime
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

        private string _selectedExerciseType;
        public string SelectedExerciseType
        {
            get { return _selectedExerciseType; }
            set
            {
                if (_selectedExerciseType != value)
                {
                    _selectedExerciseType = value;
                    OnPropertyChanged();

                    NavigateToEventFormPage();
                }
            }
        }

        private INavigation _navigation;
        public INavigation Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public ExerciseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        private void NavigateToEventFormPage()
        {
            Navigation.PushAsync(new EventFormPage());
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
