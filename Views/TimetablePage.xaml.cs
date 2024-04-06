using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Scheduler;
using TraxAct.Models;
using TraxAct.ViewModels;
using Firebase.Auth;
using FirebaseAdmin.Auth;

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
			//this.Appearing += OnPageAppearing;
		}

		//private async void OnPageAppearing(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if 
		//		{
		//			UserIdentifier = 

		//			viewModel = new TimetableViewModel(userId);
		//			BindingContext = viewModel;

		//			await viewModel.LoadEventsFromDatabase(userId);

		//			Debug.WriteLine($"Events count: {viewModel.Events.Count}");
		//			foreach (var evt in viewModel.Events)
		//			{
		//				Debug.WriteLine($"Event Subject: {evt.Subject}, Start Time: {evt.StartTime}, End Time: {evt.EndTime}");
		//			}
		//		}
		//		else
		//		{
		//			Debug.WriteLine("No user is currently signed in.");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Debug.WriteLine($"Exception while loading events: {ex.Message}");
		//	}
		//}

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

						int eventId = (int)selectedAppointment.Id;
						Debug.WriteLine($"Event ID of tapped event: {eventId}");

						if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
						{
							await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventDetailsPage(eventId));
							Debug.WriteLine("DetailsCommand execution completed successfully.");
						}
						else
						{
							Debug.WriteLine("Shell navigation is not available. DetailsCommand execution failed.");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error executing DetailsCommand: {ex.Message}");
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
	}
}
