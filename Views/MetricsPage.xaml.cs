using Syncfusion.Maui.Scheduler;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.ViewModels;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using TraxAct.Services;

namespace TraxAct.Views;

public partial class MetricsPage : ContentPage
{
	public MetricsPage()
	{

		InitializeComponent();
		BindingContext = new MetricsViewModel();
	}
}