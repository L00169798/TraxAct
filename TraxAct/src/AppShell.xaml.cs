using Microsoft.Maui.Controls;
using TraxAct.Views;
using TraxAct.Services;
using System.Diagnostics;

namespace TraxAct
{
	public partial class AppShell : Shell
	{
		private readonly UserService _userService;

		public AppShell(UserService userService)
		{
			InitializeComponent();
			Routing.RegisterRoute("HomePage", typeof(HomePage));
			Routing.RegisterRoute("SignInPage", typeof(SignInPage));
			Navigating += OnNavigating;

			_userService = userService;
		}


		private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
		{
			var shell = (Shell)sender;

			if (e.Target.Location.OriginalString == "//SignInPage")
			{
				_userService.SignOutAsync();
				Debug.WriteLine("SignOut method called.");
				shell.FlyoutBehavior = FlyoutBehavior.Disabled;
				Debug.WriteLine($"Navigating to: {e.Target.Location.OriginalString}");
			}
			else
			{
				shell.FlyoutBehavior = FlyoutBehavior.Flyout;
			}
		}
	}
}
	




