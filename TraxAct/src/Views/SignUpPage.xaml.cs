using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class SignUpPage : ContentPage
	{
		private readonly SignUpViewModel _viewModel;


		/// <summary>
		/// Constructor
		/// </summary>
		public SignUpPage()
		{
			InitializeComponent();

			_viewModel = new SignUpViewModel();

			BindingContext = _viewModel;
		}
	}
}
