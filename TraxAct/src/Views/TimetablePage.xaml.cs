using Syncfusion.Maui.Scheduler;
//using System.Diagnostics;
using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class TimetablePage : ContentPage
	{
		private TimetableViewModel viewModel;
		private readonly DateTime selectedDateTime = DateTime.Now;

		/// <summary>
		/// Constructor
		/// </summary>
		public TimetablePage()
		{
			InitializeComponent();
			this.Appearing += OnPageAppearing;
		}


		/// <summary>
		/// Method to handle page appearing event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnPageAppearing(object sender, EventArgs e)
		{
			//try
			{
				var userService = UserService.Instance;

				if (userService != null)
				{
					viewModel = new TimetableViewModel(userService);
					BindingContext = viewModel;

					await viewModel.LoadEventsFromDatabase();

					//Debug.WriteLine($"Events count: {viewModel.Events.Count}");
					foreach (var evt in viewModel.Events)
					{
						//Debug.WriteLine($"Event Subject: {evt.Subject}, Start Time: {evt.StartTime}, End Time: {evt.EndTime}");
					}
				}
				//else
				//{
				//	//Debug.WriteLine("UserService instance is null.");
				//}
			}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine($"Exception while loading events: {ex.Message}");
			//}
		}

		/// <summary>
		/// Method to handle create event button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnCreateEventButtonClicked(object sender, EventArgs e)
		{
			await NavigateToEventFormPage(selectedDateTime);
		}

		/// <summary>
		/// Method to navigate to event form page when create event button is clicked
		/// </summary>
		/// <param name="selectedDateTime"></param>
		/// <returns></returns>
		private async Task NavigateToEventFormPage(DateTime selectedDateTime)
		{
			//try
			//{
				if (viewModel?.Events?.Any(evt => evt.StartTime <= selectedDateTime && evt.EndTime > selectedDateTime) ?? false)
				{
					//Debug.WriteLine("There are existing appointments in the selected timeslot. EventFormPage will not be opened.");
					return;
				}

				if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
				{
					await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventFormPage());
					//Debug.WriteLine("Navigated to EventFormPage successfully.");
				}
				//	else
				//	{
				//		Debug.WriteLine("Shell navigation is not available. Navigation to EventFormPage failed.");
				//	}
				//}
				//catch (Exception ex)
				//{
				//	Debug.WriteLine($"Error navigating to EventFormPage: {ex.Message}");
			//}
		}

		/// <summary>
		/// Method to navigate to event details when scheduler is tapped
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
		{
			try
			{
				if (e.Appointments?.FirstOrDefault() is SchedulerAppointment selectedAppointment)
				{
					//Debug.WriteLine($"Executing DetailsCommand for event: {selectedAppointment}");
					await NavigateToEventDetails((int)selectedAppointment.Id);
				}
			}
			catch (Exception ex)
			{
				//Debug.WriteLine($"Error handling SchedulerTapped event: {ex.Message}");
			}
		}

		/// <summary>
		/// Method to navigate to event details page
		/// </summary>
		/// <param name="eventId"></param>
		/// <returns></returns>
		private static async Task NavigateToEventDetails(int eventId)
		{
			//try
			//{
			if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
			{
				await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventDetailsPage(eventId));
				//Debug.WriteLine($"Navigated to EventDetailsPage for event ID: {eventId}");
			}
			//	else
			//	{
			//		Debug.WriteLine("Shell navigation is not available. Navigation to EventDetailsPage failed.");
			//	}
			//}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine($"Error navigating to EventDetailsPage: {ex.Message}");
			//}
		}
	}
}
	
	





