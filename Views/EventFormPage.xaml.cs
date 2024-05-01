using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class EventFormPage : ContentPage
	{

		public EventFormPage()
		{
			InitializeComponent();
			BindingContext = new EventFormViewModel();
		}

	}
}