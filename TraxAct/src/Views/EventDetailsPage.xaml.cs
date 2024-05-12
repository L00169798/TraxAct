using Microsoft.Maui.Controls;
using TraxAct.Services;
using TraxAct.ViewModels;
using TraxAct.Models;
using Microsoft.Extensions.Logging;

namespace TraxAct.Views;

public partial class EventDetailsPage : ContentPage
{
    readonly MyDbContext dbContext;
    readonly EventDetailsViewModel viewModel;
    //private Event eventItem;

	public EventDetailsPage(int eventId)
	{
		InitializeComponent();
		dbContext = new MyDbContext();
		viewModel = new EventDetailsViewModel(dbContext, eventId);
		BindingContext = viewModel;
	}
}