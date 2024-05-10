using Microsoft.Maui.Controls;
using TraxAct.Views;
using TraxAct.Services;

namespace TraxAct
{
	public partial class AppShell : Shell
	{

		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute("HomePage", typeof(HomePage));
		}


		private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
		{
			if (e.Source != ShellNavigationSource.ShellItemChanged)
			{
				return;
			}

			if (e.Target.Location.OriginalString == "//HomePage")
			{
				await GoToAsync("//HomePage");
			}

		}
	}
}
