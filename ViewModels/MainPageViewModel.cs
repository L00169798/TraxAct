using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class MainPageViewModel
	{
		private readonly UserService _userService;

		public MainPageViewModel()
		{
			_userService = new UserService();
		}

		public async Task OnLogoutButtonClickedAsync()
		{
			try
			{
				var confirmLogout = await Application.Current.MainPage.DisplayAlert("Confirm Logout", "Are you sure you want to log out?", "Yes", "No");

				if (confirmLogout)
				{
					var currentUserUid = _userService.GetCurrentUserUid();

					if (!string.IsNullOrEmpty(currentUserUid))
					{
					
						bool signOutSuccess = _userService.SignOut();

						if (signOutSuccess)
						{
							
							await Application.Current.MainPage.DisplayAlert("Logged Out", "You have been successfully logged out.", "OK");
							await Shell.Current.GoToAsync("//SignInPage");
						}
						else
						{
							
							await Application.Current.MainPage.DisplayAlert("Error", "Failed to log out.", "OK");
						}
					}
					else
					{
						
						await Application.Current.MainPage.DisplayAlert("Error", "No user is currently signed in.", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during logout: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to log out.", "OK");
			}
		}
	}
}
