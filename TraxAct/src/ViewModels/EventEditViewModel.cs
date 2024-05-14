using System.ComponentModel;
//using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
	public class EventEditViewModel : INotifyPropertyChanged
	{
		// Properties
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

					if (SaveCommand is Command saveCommand)
					{
						saveCommand.ChangeCanExecute();
					}
				}
			}
		}

		private void UpdateDistanceVisibility()
		{
			IsDistanceVisible = SelectedExerciseType == "Running";
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
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			SelectedEvent = selectedEvent ?? throw new ArgumentNullException(nameof(selectedEvent));

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

			IsExerciseTypeErrorVisible = !isValidExerciseType;
			IsEndDateErrorVisible = !isEndTimeAfterStartTime;

			//Debug.WriteLine($"User UID is valid: {isValidUserUid}");
			//Debug.WriteLine($"Start DateTime: {startDateTime}");
			//Debug.WriteLine($"End DateTime: {endDateTime}");
			//Debug.WriteLine($"End DateTime is after Start DateTime: {isEndTimeAfterStartTime}");

			return isValidExerciseType && isValidUserUid && isEndTimeAfterStartTime;
		}

		/// <summary>
		/// Save Command execution
		/// </summary>
		private async void ExecuteSaveCommand()
		{
			try
			{
				//	Debug.WriteLine("Executing SaveCommand...");

				DateTime startDateTime = StartDate.Date.Add(StartTime);
				DateTime endDateTime = EndDate.Date.Add(EndTime);

				//Debug.WriteLine($"Start DateTime: {startDateTime}");
				//Debug.WriteLine($"End DateTime: {endDateTime}");

				SelectedEvent.Title = Subject;
				SelectedEvent.ExerciseType = SelectedExerciseType;
				SelectedEvent.StartTime = startDateTime;
				SelectedEvent.EndTime = endDateTime;
				SelectedEvent.Distance = Distance;
				SelectedEvent.Reps = Reps;
				SelectedEvent.Sets = Sets;

				//Debug.WriteLine("Updating SelectedEvent properties...");

				await _dbContext.Update(SelectedEvent);

				//Debug.WriteLine("Event updated in the database.");

				await Application.Current.MainPage.DisplayAlert("Success", "Event updated successfully.", "OK");

				TimetablePage timetablePage = new TimetablePage();
				await Application.Current.MainPage.Navigation.PushAsync(timetablePage);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");

				//Debug.WriteLine($"Error occurred during SaveCommand execution: {ex}");
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