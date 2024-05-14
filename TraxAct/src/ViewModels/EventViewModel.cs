using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TraxAct.ViewModels
{
	public class EventViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Event to notify about property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Selected exercise option
		/// </summary>
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

		/// <summary>
		/// Collection of exercise options
		/// </summary>
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

		/// <summary>
		/// Constructor to initialize the ViewModel
		/// </summary>
		public EventViewModel()
		{
			// Initialize the exercise options
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

		/// <summary>
		/// Method to raise the PropertyChanged event
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
