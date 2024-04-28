using TraxAct.Services;
using TraxAct.ViewModels;

namespace TraxAct.Views;

public partial class AnalysisPage : ContentPage
{
	private AnalysisViewModel _viewModel;
	private readonly IUserService _userService;

	public AnalysisPage(IUserService userService)
	{
		InitializeComponent();
		_viewModel = new AnalysisViewModel(_userService);
		_userService = userService;
		BindingContext = _viewModel;
	}

}






