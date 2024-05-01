using System;
using System.Threading.Tasks;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class HomeViewModel
	{
		private readonly UserService _userService;

		public HomeViewModel()
		{
			_userService = UserService.Instance;
		}

		public string GetCurrentUserId()
		{
			return _userService.GetCurrentUserUid();
		}
	}
}