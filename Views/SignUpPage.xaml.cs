using Firebase.Auth;
using TraxAct.ViewModels;
using TraxAct.Services;

namespace TraxAct.Views
{
	public partial class SignUpPage : ContentPage
	{
		private readonly SignUpViewModel _viewModel;

		public SignUpPage()
		{
			InitializeComponent();

			_viewModel = new SignUpViewModel();

			BindingContext = _viewModel;
		}
	}
}
