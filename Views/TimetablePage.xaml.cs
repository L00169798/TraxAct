using System;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using TraxAct.ViewModels;
using Syncfusion.Maui.Scheduler;
using System.Threading.Tasks;

namespace TraxAct.Views
{
    public partial class TimetablePage : ContentPage
    {
        private TimetableViewModel viewModel;

        public TimetablePage()
        {
            InitializeComponent();
            this.Appearing += OnPageAppearing;
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            try
            {
                viewModel = new TimetableViewModel();
                BindingContext = viewModel;

                await viewModel.LoadEventsFromDatabase();
                Debug.WriteLine($"Events count: {viewModel.Events.Count}");
                foreach (var evt in viewModel.Events)
                {
                    Debug.WriteLine($"Event Subject: {evt.Subject}, Start Time: {evt.StartTime}, End Time: {evt.EndTime}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while loading events: {ex.Message}");
            }
        }

        private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
        {
            if (e.Appointments != null && e.Appointments.Any())
            {
                var selectedAppointment = e.Appointments.First() as SchedulerAppointment;

                try
                {
                    Debug.WriteLine($"Executing DetailsCommand for event: {selectedAppointment}");

                    int eventId = (int)selectedAppointment.Id;
                    Debug.WriteLine($"Event ID of tapped event: {eventId}");

                    if (Shell.Current != null && Shell.Current.Navigation != null)
                    {
                        await Shell.Current.Navigation.PushAsync(new EventDetailsPage(eventId));
                        Debug.WriteLine("DetailsCommand execution completed successfully.");
                    }
                    else
                    {
                        Debug.WriteLine("Shell.Current or Shell.Current.Navigation is null. DetailsCommand execution failed.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error executing DetailsCommand: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("Selected appointment is null. Cannot execute DetailsCommand.");
            }
        }
    }
}
