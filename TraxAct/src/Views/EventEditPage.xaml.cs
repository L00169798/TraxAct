using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class EventEditPage : ContentPage
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewModel"></param>
		public EventEditPage(EventEditViewModel viewModel)
		{
			InitializeComponent();
			this.BindingContext = viewModel;

		}
	}
}
