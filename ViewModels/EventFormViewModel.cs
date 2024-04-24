using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.Services;
using Firebase;
using TraxAct.Views;
using Firebase.Auth.Providers;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;



namespace TraxAct.ViewModels
{
	public class EventFormViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
		private MyDbContext _dbContext;

		private Firebase.Auth.User _currentUser;
		public Firebase.Auth.User CurrentUser
		{
			get { return _currentUser; }
			set
			{
				_currentUser = value;
				OnPropertyChanged();
				((Command)SaveCommand).ChangeCanExecute();
			}
		}

		private string _userId;
		public string UserId
		{
			get { return _userId; }
			private set
			{
				_userId = value;
				OnPropertyChanged();
				((Command)SaveCommand).ChangeCanExecute();
			}
		}

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
					((Command)SaveCommand).ChangeCanExecute();
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
					UpdateVisibility();
				}
			}
		}

		private string _errorMessage;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				_errorMessage = value;
				OnPropertyChanged();
			}
		}


		private void UpdateVisibility()
		{
			if (SelectedExerciseType == "Running" || SelectedExerciseType == "Walking" || SelectedExerciseType == "Cycling")
			{
				IsDistanceVisible = true;
			}
			else
			{
				IsDistanceVisible = false;
			}
			if (SelectedExerciseType == "Strength")
			{
				IsRepsVisible = true;
				IsSetsVisible = true;
			}
			else
			{
				IsRepsVisible = false;
				IsSetsVisible = false;
			}
		}


		private DateTime _startDate = DateTime.Today;
		public DateTime StartDate
		{
			get { return _startDate; }
			set
			{
				if (_startDate != value)
				{
					_startDate = value;
					OnPropertyChanged();
					((Command)SaveCommand).ChangeCanExecute();
				}
			}
		}

		private DateTime _endDate = DateTime.Today;
		public DateTime EndDate
		{
			get { return _endDate; }
			set
			{
				if (_endDate != value)
				{
					_endDate = value;
					OnPropertyChanged();
					((Command)SaveCommand).ChangeCanExecute();
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
					((Command)SaveCommand).ChangeCanExecute();
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
					((Command)SaveCommand).ChangeCanExecute();
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

		private bool _isDistanceVisible = true;
		public bool IsDistanceVisible
		{
			get { return _isDistanceVisible; }
			set
			{
				if (_isDistanceVisible != value)
				{
					_isDistanceVisible = value;
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

		private bool _isRepsVisible;
		public bool IsRepsVisible
		{
			get { return _isRepsVisible; }
			set
			{
				if (_isRepsVisible != value)
				{
					_isRepsVisible = value;
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
					OnPropertyChanged();
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
			"Walking", "Running", "Cycling", "Swimming", "Yoga",
			"Pilates", "Strength", "HIIT", "Circuit", "Other"
		};

		public ICommand SaveCommand { get; }

		public EventFormViewModel()
		{
			_dbContext = new MyDbContext();

			SaveCommand = new Command(async () => await ExecuteSaveCommand(), () => CanExecuteSaveCommand());

			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(Subject))
				{
					((Command)SaveCommand).ChangeCanExecute();
					ErrorMessage = string.Empty;
				}
			};
		}

		private bool CanExecuteSaveCommand()
		{
			DateTime startDateTime = StartDate.Date.Add(StartTime);
			DateTime endDateTime = EndDate.Date.Add(EndTime);

			bool isValidExerciseType = !string.IsNullOrWhiteSpace(SelectedExerciseType);
			bool isValidUserUid = !string.IsNullOrWhiteSpace(UserService.Instance.GetCurrentUserUid());
			bool isEndTimeAfterStartTime = endDateTime >= startDateTime;

			IsExerciseTypeErrorVisible = !isValidExerciseType;
			IsEndDateErrorVisible = !isEndTimeAfterStartTime;

			bool canExecute = isValidExerciseType && isValidUserUid && isEndTimeAfterStartTime;

			Debug.WriteLine($"User UID is valid: {isValidUserUid}");
			Debug.WriteLine($"Start DateTime: {startDateTime}");
			Debug.WriteLine($"End DateTime: {endDateTime}");
			Debug.WriteLine($"End DateTime is after Start DateTime: {isEndTimeAfterStartTime}");

			return isValidExerciseType && isValidUserUid && isEndTimeAfterStartTime;
		}

		private async Task ExecuteSaveCommand()
		{
			try
			{
				Debug.WriteLine("Executing Save Command...");

				string currentUserUid = UserService.Instance.GetCurrentUserUid();

				DateTime startDateTime = StartDate.Date.Add(StartTime);
				DateTime endDateTime = EndDate.Date.Add(EndTime);
				Debug.WriteLine($"Start DateTime (before saving to database): {startDateTime}");
				Debug.WriteLine($"End DateTime (before saving to database): {endDateTime}");

				Event newEvent = new Event
				{
					Title = Subject,
					UserId = currentUserUid,
					StartTime = startDateTime,
					EndTime = endDateTime,
					ExerciseType = SelectedExerciseType,
					Distance = Distance,
					Sets = Sets,
					Reps = Reps
				};

				bool result = await _dbContext.Create(newEvent);

				if (result)
				{
					Debug.WriteLine("Event saved successfully.");
					await Application.Current.MainPage.Navigation.PopAsync();
				}
				else
				{
					Debug.WriteLine("Failed to save event.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error saving event: {ex.Message}");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}