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

		private void UpdateVisibility()
		{
			IsDistanceVisible = SelectedExerciseType == "Running";
			IsRepsVisible = SelectedExerciseType == "Strength";
			IsSetsVisible = SelectedExerciseType == "Strength";
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
				}
			};
		}

		private bool CanExecuteSaveCommand()
		{
			return !string.IsNullOrWhiteSpace(Subject) && !string.IsNullOrWhiteSpace(UserService.Instance.GetCurrentUserUid());
		}

		private async Task ExecuteSaveCommand()
		{
			try
			{
				Debug.WriteLine("Executing Save Command...");

				if (string.IsNullOrWhiteSpace(Subject))
				{
					Debug.WriteLine("Validation failed. Please check your inputs.");
					return;
				}

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