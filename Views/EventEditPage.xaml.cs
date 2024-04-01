using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.ViewModels;
using TraxAct.Services;

namespace TraxAct.Views
{
    public partial class EventEditPage : ContentPage
    {
        public EventEditPage(EventEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            Event editedEvent = viewModel.SelectedEvent;
        }
    }
}
