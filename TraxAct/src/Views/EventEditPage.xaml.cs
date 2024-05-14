using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
	public partial class EventEditPage : ContentPage
	{
		MyDbContext dbContext;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewModel"></param>
		public EventEditPage(EventEditViewModel viewModel)
		{
			InitializeComponent();
			dbContext = new MyDbContext();
			this.BindingContext = viewModel;

		}
	}
}
