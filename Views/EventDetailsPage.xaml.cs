using TraxAct.Models;
using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views;

public partial class EventDetailsPage : ContentPage
{
	MyDbContext dbContext;
	EventDetailsViewModel viewModel;
	private Event eventItem;

	public EventDetailsPage(int eventId)
	{
		InitializeComponent();
		dbContext = new MyDbContext();
		viewModel = new EventDetailsViewModel(dbContext, eventId);
		BindingContext = viewModel;
	}
}