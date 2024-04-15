using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TraxAct.Models;

namespace TraxAct.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedExerciseOption;
        public string SelectedExerciseOption
        {
            get { return _selectedExerciseOption; }
            set
            {
                if (_selectedExerciseOption != value)
                {
                    _selectedExerciseOption = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _exerciseOptions;
        public ObservableCollection<string> ExerciseOptions
        {
            get { return _exerciseOptions; }
            set
            {
                if (_exerciseOptions != value)
                {
                    _exerciseOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        public EventViewModel()
        {
            ExerciseOptions = new ObservableCollection<string>
    {
        "Walking",
        "Running",
        "Cycling",
        "Swimming",
        "Yoga",
        "Pilates",
        "Strength",
        "HIIT",
        "Circuit",
        "Other"
    };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
