using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.ViewModels;
using TraxAct.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Maui.Core.Carousel;

namespace TraxAct.Views
{
    public partial class EventEditPage : ContentPage
    {
        MyDbContext dbContext;
        //EventEditViewModel viewModel;
        //private Event eventItem;
        public EventEditPage(EventEditViewModel viewModel)
        {
            InitializeComponent();
            dbContext = new MyDbContext();
            this.BindingContext = viewModel;
        
        }
    }
}
