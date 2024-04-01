using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using TraxAct.ViewModels;
using TraxAct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TraxAct.Services;
using System.Diagnostics;

namespace TraxAct.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        MyDbContext dbContext;
        CalendarViewModel viewModel;
        private Event eventItem;

        public CalendarPage()
        {
            InitializeComponent();
            dbContext = new MyDbContext();
            viewModel = new CalendarViewModel(dbContext);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadEvents(viewModel.CurrentDate);
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.LoadEvents(e.NewDate);
        }


        private async void OnCreateTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new EventFormPage());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        private async void OnDetailsTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            try
            {
                var tappedEventArgs = e as TappedEventArgs;
                if (tappedEventArgs == null)
                {
                    Debug.WriteLine("Invalid TappedEventArgs: null");
                    return;
                }

                var eventItem = tappedEventArgs.Parameter as Event;
                if (eventItem == null)
                {
                    Debug.WriteLine("Invalid event item: null");
                    return;
                }

                if (eventItem.EventId != 0)
                {
                    var eventDetailsViewModel = new EventDetailsViewModel(dbContext, eventItem);

                    await eventDetailsViewModel.LoadEventDetails(eventItem.EventId);

                    await Navigation.PushAsync(new EventDetailsPage(eventItem));
                }
                else
                {
                    Console.WriteLine("Invalid event ID.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }




    }

}

