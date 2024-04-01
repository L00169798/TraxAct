using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views
{
    public partial class EventPage : ContentPage
    {
        private readonly MyDbContext _dbContext;
        public EventPage(MyDbContext dbContext)
        {
            InitializeComponent();
            BindingContext = new EventViewModel();
        }
    }
}
