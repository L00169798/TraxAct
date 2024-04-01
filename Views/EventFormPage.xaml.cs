using Microsoft.Maui.Controls;
using TraxAct.Services;
using TraxAct.ViewModels;
using TraxAct.Models;

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
