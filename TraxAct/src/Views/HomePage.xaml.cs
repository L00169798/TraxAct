using TraxAct.ViewModels;

namespace TraxAct.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		private HomePageViewModel _viewModel;

		/// <summary>
		/// Constructor
		/// </summary>
		public HomePage()
		{
			InitializeComponent();
			_viewModel = new HomePageViewModel();
			BindingContext = _viewModel;
		}

		/// <summary>
		/// Method invoked when the page is appearing on the screen
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();
			Console.WriteLine("HomePage is appearing. The button should be interactable.");
		}

		/// <summary>
		/// Event handler for the timetable button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Event handler for the analysis button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
	}
}


