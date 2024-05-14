using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class SignInPage : ContentPage
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SignInPage()
		{
			InitializeComponent();

			BindingContext = new SignInViewModel();
		}
	}
}
