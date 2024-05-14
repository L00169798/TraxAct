using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class HomePageViewModel
	{
		private readonly UserService _userService;

		/// <summary>
		/// Constructor
		/// </summary>
		public HomePageViewModel()
		{
			_userService = UserService.Instance;
		}

		/// <summary>
		/// Get logged in User Id
		/// </summary>
		/// <returns></returns>
		public string GetCurrentUserId()
		{
			return _userService.GetCurrentUserUid();
		}
	}
}