using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct
{
	public partial class AppShell : Shell
	{
		private readonly UserService _userService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userService"></param>
		public AppShell(UserService userService)
		{
			InitializeComponent();
			Routing.RegisterRoute("HomePage", typeof(HomePage));
			Routing.RegisterRoute("SignInPage", typeof(SignInPage));
			Navigating += OnNavigating;

			_userService = userService;
		}

		/// <summary>
		/// Method to trigger signout method and disable flyout menu when navigating back to sign in page after logout
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
		{
			var shell = (Shell)sender;

			if (e.Target.Location.OriginalString == "//SignInPage")
			{
				await _userService.SignOutAsync();
				shell.FlyoutBehavior = FlyoutBehavior.Disabled;
			}
			else
			{
				shell.FlyoutBehavior = FlyoutBehavior.Flyout;
			}
		}
	}
}





