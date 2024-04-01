using System;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using TraxAct.ViewModels;
using TraxAct.Models;
using Syncfusion.Maui.Scheduler;


namespace TraxAct.Views
{
    public partial class TimetablePage : ContentPage
    {
        private TimetableViewModel viewModel;

        public TimetablePage()
        {
            try
            {
                InitializeComponent();
                viewModel = new TimetableViewModel();
                BindingContext = viewModel;

                Task.Run(async () =>
                {
                    await viewModel.LoadEventsFromDatabase();
                    Console.WriteLine($"Events count: {viewModel.Events.Count}");
                    foreach (var evt in viewModel.Events)
                    {
                        Console.WriteLine($"Event Subject: {evt.Subject}, Start Time: {evt.StartTime}, End Time: {evt.EndTime}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in constructor: {ex.Message}");
            }
        }

        private async void OnSchedulerTapped(object sender, Syncfusion.Maui.Scheduler.SchedulerTappedEventArgs e)
        {
            if (e.Appointments != null && e.Appointments.Any())
            {
                var selectedAppointment = e.Appointments.First() as SchedulerAppointment;

                try
                {
                    Debug.WriteLine($"Executing DetailsCommand for event: {selectedAppointment}");

                    var selectedEvent = new Event
                    {
                        Subject = selectedAppointment.Subject,
                        StartTime = selectedAppointment.StartTime,
                        EndTime = selectedAppointment.EndTime
                    };

                    if (Shell.Current != null && Shell.Current.Navigation != null)
                    {
                        await Shell.Current.Navigation.PushAsync(new EventDetailsPage(selectedEvent));
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
