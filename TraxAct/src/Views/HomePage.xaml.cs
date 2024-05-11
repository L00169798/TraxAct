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
    public partial class HomePage : ContentPage
    {
		private HomePageViewModel _viewModel;

		public HomePage()
		{
			InitializeComponent();
			_viewModel = new HomePageViewModel();
			BindingContext = _viewModel;
		}

		protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("HomePage is appearing. The button should be interactable.");
        }


        private async void OnTimetableTapped(object sender, EventArgs e)
        {
            if (Navigation != null)
            {
				await Shell.Current.GoToAsync($"///{nameof(TimetablePage)}");
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
				await Shell.Current.GoToAsync($"///{nameof(AnalysisPage)}");
			}
			else
			{
				await DisplayAlert("Error", "Navigation is not available.", "OK");
			}
		}

		//private async void OnLogoutButtonClicked(object sender, EventArgs e)
		//{
		//	if (_viewModel != null)
		//	{
		//		await _viewModel.OnLogoutButtonClickedAsync();
		//	}
		//	else
		//	{
		//		Console.WriteLine("_viewModel is null");
		//	}
		//}


	}
}
    

