using Microsoft.Maui.Controls;
using TraxAct.Services;
using TraxAct.ViewModels;
using TraxAct.Models;
using Microsoft.Extensions.Logging;

namespace TraxAct.Views;

public partial class EventDetailsPage : ContentPage
{
    MyDbContext dbContext;
    EventDetailsViewModel viewModel;
    private Event eventItem;

    public EventDetailsPage(Event eventId)
    {
        InitializeComponent();
        dbContext = new MyDbContext();
        viewModel = new EventDetailsViewModel(dbContext, eventId);
        BindingContext = viewModel;
    }
}