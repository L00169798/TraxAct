using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class HomePageViewModel
	{
		private readonly UserService _userService;

		public HomePageViewModel()
		{
			_userService = UserService.Instance;
		}

		public string GetCurrentUserId()
		{
			return _userService.GetCurrentUserUid();
		}
	}
}