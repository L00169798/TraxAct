using TraxAct.Models;
using TraxAct.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using TraxAct;

public class FlyoutMenuPage : ContentPage
{
    public ListView ListView { get; } = new ListView();

    public FlyoutMenuPage()
    {
        var flyoutPageItems = new List<FlyoutMenuItem>
        {
            new FlyoutMenuItem { Title = "Home", TargetType = typeof(MainPage) },
            new FlyoutMenuItem { Title = "Calendar", TargetType = typeof(TimetablePage) }
        };

        ListView.ItemsSource = flyoutPageItems;
        ListView.ItemSelected += OnItemSelected;

        Content = new StackLayout
        {
            Children = { ListView }
        };
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is FlyoutMenuItem item)
        {
            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            var mainPage = Application.Current.MainPage as FlyoutPage;
            mainPage.Detail = new NavigationPage(page);
            mainPage.IsPresented = false; 

            ListView.SelectedItem = null;
        }
    }
}
