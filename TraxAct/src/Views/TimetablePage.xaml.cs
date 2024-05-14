using Syncfusion.Maui.Scheduler;
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
			await LoadTimetableViewModel();
		}

		private async Task LoadTimetableViewModel()
		{
			var userService = UserService.Instance;

			if (userService != null)
			{
				viewModel = new TimetableViewModel(userService);
				BindingContext = viewModel;

				await viewModel.LoadEventsFromDatabase();
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Please try again later.", "OK");
			}
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
				if (viewModel?.Events?.Any(evt => evt.StartTime <= selectedDateTime && evt.EndTime > selectedDateTime) ?? false)
				{
					return;
				}

				if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
				{
					await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventFormPage());
				}
			    else
			    {
				await Application.Current.MainPage.DisplayAlert("Error", "Navigation failed, Please try again later.", "OK");
			    }
		}

		/// <summary>
		/// Method to navigate to event details when scheduler is tapped
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
		{
				if (e.Appointments?.FirstOrDefault() is SchedulerAppointment selectedAppointment)
				{
					await NavigateToEventDetails((int)selectedAppointment.Id);
				}
			    else
			    {
				await Application.Current.MainPage.DisplayAlert("Error", "Please try again later.", "OK");
			    }
		}

		/// <summary>
		/// Method to navigate to event details page
		/// </summary>
		/// <param name="eventId"></param>
		/// <returns></returns>
		private static async Task NavigateToEventDetails(int eventId)
		{
			if (Application.Current.MainPage is Shell shell && shell.CurrentItem?.CurrentItem?.CurrentItem?.Navigation != null)
			{
				await shell.CurrentItem.CurrentItem.CurrentItem.Navigation.PushAsync(new EventDetailsPage(eventId));
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Navigation failed. Please try again later.", "OK");
			}
		}
	}
}
	
	





