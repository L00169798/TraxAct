using Syncfusion.Maui.Scheduler;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Controls;
using TraxAct.Models;
using TraxAct.ViewModels;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using TraxAct.Services;
using Syncfusion.Maui.Core.Carousel;

namespace TraxAct.Views;

public partial class MetricsPage : ContentPage
{
	private MetricsViewModel _viewModel;

	public MetricsPage()
	{
		InitializeComponent();
		_viewModel = new MetricsViewModel();
		BindingContext = _viewModel;
	}

}






