﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TraxAct.Models;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
    public class EventDetailsViewModel : INotifyPropertyChanged
    {
        private readonly MyDbContext _dbContext;
        private Event _selectedEvent;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    OnPropertyChanged(nameof(SelectedEvent));
                }
            }
        }

        public EventDetailsViewModel(MyDbContext dbContext, Event eventItem)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            }

            _dbContext = dbContext;
            SelectedEvent = eventItem;
            EditCommand = new Command(EditButton_Clicked);
            DeleteCommand = new Command(async () => await DeleteButton_Clicked());
        }

        public async Task<bool> LoadEventDetails(int eventId)
        {
            Console.WriteLine("LoadEventDetails method called.");

            try
            {
                SelectedEvent = await _dbContext.GetById(eventId);

                if (SelectedEvent == null)
                {
                    Console.WriteLine($"Event with ID {eventId} not found.");
                    return false;
                }
                else
                {
                    Console.WriteLine("Event details loaded successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading event details: {ex.Message}");
                return false;
            }
        }

        private async void EditButton_Clicked()
        {
            try
            {
                if (SelectedEvent != null)
                {
                    Debug.WriteLine($"Editing event with ID: {SelectedEvent.EventId}");

                    var mainPage = App.Current.MainPage as NavigationPage;

                    if (mainPage != null && mainPage.Navigation != null)
                    {

                        await mainPage.Navigation.PushAsync(new EventEditPage(new EventEditViewModel(_dbContext, SelectedEvent)));
                    }
                    else
                    {
                        Debug.WriteLine("MainPage or its Navigation is null.");
                    }
                }
                else
                {
                    Debug.WriteLine("Cannot edit event details: SelectedEvent is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error editing event details: {ex.Message}");
            }
        }

        private async Task DeleteButton_Clicked()
        {
            if (SelectedEvent != null)
            {
                try
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to delete this event?", "Yes", "No");
                    if (answer)
                    {
                        await _dbContext.Delete(SelectedEvent);
                        Debug.WriteLine("Event deleted successfully.");

                        await Application.Current.MainPage.DisplayAlert("Success", "Event has been deleted successfully.", "OK");

                        await Shell.Current.Navigation.PopAsync();

                        SelectedEvent = null;
                    }
                    else
                    {
                        Debug.WriteLine("Delete operation canceled by the user.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting event: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while deleting the event.", "OK");
                    Console.WriteLine($"Error in DeleteButton_Clicked: {ex.Message}");
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}