using System.Diagnostics;
using TraxAct.Services;
using TraxAct.Views;


namespace TraxAct
{
	public partial class AppShell : Shell
	{
		private readonly UserService _userService;
		//public ICommand LogoutCommand { get; private set; }
		public AppShell(UserService userService)
		{
			InitializeComponent();
			RegisterRoutes();
			_userService = userService;

			//LogoutCommand = new Command(ExecuteLogoutCommand);


		}

		private void RegisterRoutes()
		{
			Routing.RegisterRoute(nameof(TimetablePage), typeof(TimetablePage));
		}


		//private async Task OnLogoutTapped(object sender, EventArgs e, UserService userService)
		//{
		//	Debug.WriteLine($"Logout event executed");
		//	try
		//	{
		//		var confirmLogout = await DisplayAlert("Confirm Logout", "Are you sure you want to log out?", "Yes", "No");

		//		if (confirmLogout)
		//		{
		//			bool signOutSuccess = _userService.SignOut();

		//			if (signOutSuccess)
		//			{
		//				_userService.SetCurrentUserUid(null);
		//				await DisplayAlert("Logged Out", "You have been successfully logged out.", "OK");

		//				await Shell.Current.GoToAsync("//SignIn");
		//			}
		//			else
		//			{
		//				await DisplayAlert("Error", "Failed to log out.", "OK");
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Debug.WriteLine($"Error during logout: {ex.Message}");
		//		await DisplayAlert("Error", "An error occurred during logout.", "OK");
		//	}
		//}
	}
}

