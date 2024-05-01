using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class TimetablePage : ContentPage
	{
		private TimetableViewModel viewModel;
		private DateTime selectedDateTime;
		private FirebaseServices _firebaseServices;

		public TimetablePage()
		{
			InitializeComponent();
			_firebaseServices = new FirebaseServices();
			this.Appearing += OnPageAppearing;
		}

		private async void OnPageAppearing(object sender, EventArgs e)
		{
			try
			{
				var userService = UserService.Instance;

				if (userService != null)
				{
					viewModel = new TimetableViewModel(userService);
					BindingContext = viewModel;

					await viewModel.LoadEventsFromDatabaseAsync();

					Debug.WriteLine($"Events count: {viewModel.Events.Count}");
					foreach (var evt in viewModel.Events)
					{
						Debug.WriteLine($"Event Subject: {evt.Subject}, Start Time: {evt.StartTime}, End Time: {evt.EndTime}");
					}
				}
				else
				{
					Debug.WriteLine("UserService instance is null.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception while loading events: {ex.Message}");
			}
		}


		private async void OnCreateEventButtonClicked(object sender, EventArgs e)
		{
			await NavigateToEventFormPage(selectedDateTime);
		}


		private async Task NavigateToEventFormPage(DateTime selectedDateTime)
		{
			try
			{
				if (viewModel?.Events?.Any(evt => evt.StartTime <= selectedDateTime && evt.EndTime > selectedDateTime) ?? false)
				{
					Debug.WriteLine("There are existing appointments in the selected timeslot. EventFormPage will not be opened.");
					return;
				}

				if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
				{
					await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventFormPage());
					Debug.WriteLine("Navigated to EventFormPage successfully.");
				}
				else
				{
					Debug.WriteLine("Shell navigation is not available. Navigation to EventFormPage failed.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error navigating to EventFormPage: {ex.Message}");
			}
		}

		private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
		{
			try
			{

				if (e.Appointments != null && e.Appointments.Any())
				{

					var selectedAppointment = e.Appointments.First() as SchedulerAppointment;

					if (selectedAppointment != null)
					{
						Debug.WriteLine($"Executing DetailsCommand for event: {selectedAppointment}");


						await NavigateToEventDetails((int)selectedAppointment.Id);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error handling SchedulerTapped event: {ex.Message}");
			}
		}

		private async Task NavigateToEventDetails(int eventId)
		{
			try
			{
				if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
				{
					await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventDetailsPage(eventId));
					Debug.WriteLine($"Navigated to EventDetailsPage for event ID: {eventId}");
				}
				else
				{
					Debug.WriteLine("Shell navigation is not available. Navigation to EventDetailsPage failed.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error navigating to EventDetailsPage: {ex.Message}");
			}
		}
	}
}




