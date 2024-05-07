using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls;
using TraxAct.Services;
using TraxAct.ViewModels;
using TraxAct.Models;
using Firebase.Auth;
using Syncfusion.Maui.Core.Carousel;

namespace TraxAct.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
		private MainPageViewModel _viewModel;

		public MainPage()
		{
			InitializeComponent();
			_viewModel = new MainPageViewModel();
			BindingContext = _viewModel;
		}

		protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("MainPage is appearing. The button should be interactable.");
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

		private async void OnAnalysisTapped(object sender, EventArgs e)
		{
			if (Navigation != null)
			{
				await Navigation.PushAsync(new MetricsPage());
			}
			else
			{
				await DisplayAlert("Error", "Navigation is not available.", "OK");
			}
		}

		private async void OnLogoutButtonClicked(object sender, EventArgs e)
		{
			if (_viewModel != null)
			{
				await _viewModel.OnLogoutButtonClickedAsync();
			}
			else
			{
				Console.WriteLine("_viewModel is null");
			}
		}


	}
}
    

