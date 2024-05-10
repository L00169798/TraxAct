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
	}
}
