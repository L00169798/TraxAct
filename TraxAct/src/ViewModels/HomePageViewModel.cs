using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class HomePageViewModel
	{
		private readonly UserService _userService;

		public HomePageViewModel()
		{
			_userService = UserService.Instance;
		}

		//public async Task OnLogoutButtonClickedAsync()
		//{
		//	try
		//	{
		//		var confirmLogout = await Application.Current.MainPage.DisplayAlert("Confirm Logout", "Are you sure you want to log out?", "Yes", "No");

		//		if (confirmLogout)
		//		{
		//			bool signOutSuccess = _userService.SignOut();

		//			if (signOutSuccess)
		//			{
						
		//				_userService.SetCurrentUserUid(null);

					
		//				await Application.Current.MainPage.DisplayAlert("Logged Out", "You have been successfully logged out.", "OK");

						
		//				await Shell.Current.GoToAsync("//SignIn");
		//			}
		//			else
		//			{
		//				await Application.Current.MainPage.DisplayAlert("Error", "Failed to log out.", "OK");
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error during logout: {ex.Message}");
		//		await Shell.Current.GoToAsync($"///{nameof(SignInPage)}").DisplayAlert("Error", "An error occurred during logout.", "OK");
		//	}
		//}

		public string GetCurrentUserId()
		{
			return _userService.GetCurrentUserUid();
		}
	}
}