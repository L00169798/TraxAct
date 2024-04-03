using System;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using TraxAct.ViewModels;
using Syncfusion.Maui.Scheduler;
using System.Threading.Tasks;
using TraxAct.Models;

namespace TraxAct.Views
{
    public partial class TimetablePage : ContentPage
    {
        private TimetableViewModel viewModel;
        private DateTime selectedDateTime;

        public object Events { get; private set; }

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

                viewModel?.ReloadEventsFromDatabase().ConfigureAwait(false);
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

        private async void OnCreateEventButtonClicked(object sender, EventArgs e)
        {
            await NavigateToEventFormPage(selectedDateTime);
        }
    


        private async Task NavigateToEventFormPage(DateTime selectedDateTime)
        {
            try
            {
                if (viewModel.Events.Any(evt => evt.StartTime <= selectedDateTime && evt.EndTime > selectedDateTime))

                {
                    Debug.WriteLine("There are existing appointments in the selected timeslot. EventFormPage will not be opened.");
                    return;
                }

                if (Shell.Current != null && Shell.Current.Navigation != null)
                {
                    await Shell.Current.Navigation.PushAsync(new EventFormPage());
                    Debug.WriteLine("Navigated to EventFormPage successfully.");
                }
                else
                {
                    Debug.WriteLine("Shell.Current or Shell.Current.Navigation is null. Navigation to EventFormPage failed.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to EventFormPage: {ex.Message}");
            }
        }
    

    }
}
