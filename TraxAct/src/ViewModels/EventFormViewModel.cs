﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;



namespace TraxAct.ViewModels
{
	public class EventFormViewModel : INotifyPropertyChanged
	{
		//Properties
		private readonly MyDbContext _dbContext;

		//Retrieve current user from Firebase
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

		//Data visibility based on selected exercise
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
				if (Math.Abs(_distance - value) > double.Epsilon)
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

		//Collection of events
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

		//List of exercise types
		public List<string> ExerciseTypes { get; } = new List<string>
		{
			"Walking", "Running", "Cycling", "Swimming", "Yoga",
			"Pilates", "Strength", "HIIT", "Circuit", "Other"
		};

		/// <summary>
		/// Command to save changes
		/// </summary>
		private ICommand _saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new Command(
						execute: async () => await ExecuteSaveCommand(),
						canExecute: () => CanExecuteSaveCommand());
				}
				return _saveCommand;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public EventFormViewModel()
		{
			_dbContext = new MyDbContext();

			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(Subject))
				{
					((Command)SaveCommand).ChangeCanExecute();
				}
			};
		}

		/// <summary>
		/// Can execute command
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
		/// Execute Save
		/// </summary>
		/// <returns></returns>
		private async Task ExecuteSaveCommand()
		{
			try
			{
				string currentUserUid = UserService.Instance.GetCurrentUserUid();

				DateTime startDateTime = StartDate.Date.Add(StartTime);
				DateTime endDateTime = EndDate.Date.Add(EndTime);

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
					await Application.Current.MainPage.Navigation.PopAsync();
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to save the event.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
			}
		}

		/// <summary>
		/// Event Handler for property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}