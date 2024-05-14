using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class EventFormPage : ContentPage
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public EventFormPage()
		{
			InitializeComponent();
			BindingContext = new EventFormViewModel();
		}

	}
}