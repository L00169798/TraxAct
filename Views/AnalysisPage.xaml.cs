using TraxAct.ViewModels;

namespace TraxAct.Views;

public partial class AnalysisPage : ContentPage
{
	private AnalysisViewModel _viewModel;

	public AnalysisPage()
	{
		InitializeComponent();
		_viewModel = new AnalysisViewModel();
		BindingContext = _viewModel;
	}

}






