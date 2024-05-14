using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
	public class EventEditViewModel : INotifyPropertyChanged
	{
		// Fields
		private readonly MyDbContext _dbContext;
		private Event _selectedEvent;

		//Events
		public event PropertyChangedEventHandler PropertyChanged;

		//Properties
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

					//Updates elements based on the selected exercise
					UpdateDistanceVisibility();
					UpdateRepsVisibility();
					UpdateSetsVisibility();

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
				}
			}
		}
		
		//Additional fields base on selected exercise type
		private void UpdateDistanceVisibility()
		{
			IsDistanceVisible = SelectedExerciseType == "Running" || SelectedExerciseType == "Walking" || SelectedExerciseType == "Cycling";
		}

		private void UpdateRepsVisibility()
		{
			IsRepsVisible = SelectedExerciseType == "Strength";
		}

		private void UpdateSetsVisibility()
		{
			IsSetsVisible = SelectedExerciseType == "Strength";
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

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
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

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
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

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
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

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
				}
			}
		}

		private double _distance;
		public double Distance
		{
			get { return _distance; }
			set
			{
				double tolerance = 0.0001;

				if (Math.Abs(_distance - value) > tolerance)
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

		private bool _isExerciseTypeErrorVisible;
		public bool IsExerciseTypeErrorVisible
		{
			get { return _isExerciseTypeErrorVisible; }
			set
			{
				if (_isExerciseTypeErrorVisible != value)
				{
					_isExerciseTypeErrorVisible = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isEndDateErrorVisible;
		public bool IsEndDateErrorVisible
		{
			get { return _isEndDateErrorVisible; }
			set
			{
				if (_isEndDateErrorVisible != value)
				{
					_isEndDateErrorVisible = value;
					OnPropertyChanged();
				}
			}
		}

		//Command to save changes
		public ICommand SaveCommand { get; private set; }


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="selectedEvent"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public EventEditViewModel(MyDbContext dbContext, Event selectedEvent)
		{
			try
			{
				_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
				SelectedEvent = selectedEvent ?? throw new ArgumentNullException(nameof(selectedEvent));

				//Assign fields
				Subject = selectedEvent.Title;
				SelectedExerciseType = selectedEvent.ExerciseType;
				StartDate = selectedEvent.StartTime;
				EndDate = selectedEvent.EndTime;
				Distance = selectedEvent.Distance;
				Reps = selectedEvent.Reps;
				Sets = selectedEvent.Sets;

				SaveCommand = new Command(ExecuteSaveCommand, CanExecuteSaveCommand);

				PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(Subject) || args.PropertyName == nameof(SelectedExerciseType))
					{
						((Command)SaveCommand).ChangeCanExecute();
					}
				};
			}
			catch 
			{
				Application.Current.MainPage.DisplayAlert("Error", "An error occurred during initialization. Please try again later.", "OK");
			}
		}

		/// <summary>
		/// Can Execute command
		/// </summary>
		/// <returns></returns>
		private bool CanExecuteSaveCommand()
		{
			DateTime startDateTime = StartDate.Date.Add(StartTime);
			DateTime endDateTime = EndDate.Date.Add(EndTime);

			bool isValidExerciseType = !string.IsNullOrWhiteSpace(SelectedExerciseType);
			bool isValidUserUid = !string.IsNullOrWhiteSpace(UserService.Instance.GetCurrentUserUid());
			bool isEndTimeAfterStartTime = endDateTime > startDateTime;

			//Input validation check
			IsExerciseTypeErrorVisible = !isValidExerciseType;
			IsEndDateErrorVisible = !isEndTimeAfterStartTime;

			return isValidExerciseType && isValidUserUid && isEndTimeAfterStartTime;
		}

		/// <summary>
		/// Save Command execution
		/// </summary>
		private async void ExecuteSaveCommand()
		{
			try
			{
				DateTime startDateTime = StartDate.Date.Add(StartTime);
				DateTime endDateTime = EndDate.Date.Add(EndTime);

				SelectedEvent.Title = Subject;
				SelectedEvent.ExerciseType = SelectedExerciseType;
				SelectedEvent.StartTime = startDateTime;
				SelectedEvent.EndTime = endDateTime;
				SelectedEvent.Distance = Distance;
				SelectedEvent.Reps = Reps;
				SelectedEvent.Sets = Sets;

				await _dbContext.Update(SelectedEvent);

				await Application.Current.MainPage.DisplayAlert("Success", "Event updated successfully.", "OK");

				//Navigate back to timetable after event is created
				TimetablePage timetablePage = new TimetablePage();
				await Application.Current.MainPage.Navigation.PushAsync(timetablePage);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
			}
		}

		/// <summary>
		/// Event handler for property change event
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}