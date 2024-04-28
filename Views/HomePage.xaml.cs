using TraxAct.ViewModels;
using TraxAct.Services;

namespace TraxAct.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		private HomeViewModel _viewModel;
		private readonly IUserService _userService;

		public HomePage()
		{
			InitializeComponent();
			_viewModel = new HomeViewModel();
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
				await Navigation.PushAsync(new AnalysisPage(_userService));
			}
			else
			{
				await DisplayAlert("Error", "Navigation is not available.", "OK");
			}
		}
	}
}


