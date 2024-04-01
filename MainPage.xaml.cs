using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using TraxAct.Views;
using TraxAct.Services;
using Microsoft.EntityFrameworkCore;

namespace TraxAct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("MainPage is appearing. The button should be interactable.");
        }

        private async void OnCalendarTapped(object sender, EventArgs e)
        {
            if (Navigation != null)
            {
                await Navigation.PushAsync(new CalendarPage());
            }
            else
            {
                await DisplayAlert("Error", "Navigation is not available.", "OK");
            }
        }

        private async void OnTimetableTapped(object sender, EventArgs e)
        {
            if (Navigation != null)
            {
                await Navigation.PushAsync(new TimetablePage());
            }
            else
            {
                await DisplayAlert("Error", "Navigation is not available.", "OK");
            }
        }

        private void OnCounterClicked(object sender, System.EventArgs e)
        {
        }
    }
}
