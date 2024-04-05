using Firebase.Auth;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class SignUpPage : ContentPage
	{
		public SignUpPage()
		{
			InitializeComponent();

			BindingContext = new SignUpViewModel();
		}
	}
}
