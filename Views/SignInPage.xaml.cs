using Firebase.Auth;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent();

			BindingContext = new SignInViewModel();
		}
	}
}
