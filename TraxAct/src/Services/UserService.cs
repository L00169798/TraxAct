namespace TraxAct.Services
{
	/// <summary>
	/// Service class for managing user-related functionality.
	/// </summary>
	public class UserService
	{
		private static UserService _instance;
		private string _currentUserUid;

		/// <summary>
		/// Gets the singleton instance of the UserService.
		/// </summary>
		public static UserService Instance => _instance ??= new UserService();

		/// <summary>
		/// Gets the UID of the current user.
		/// </summary>
		/// <returns>The UID of the current user.</returns>
		public virtual string GetCurrentUserUid()
		{
			return _currentUserUid;
		}

		/// <summary>
		/// Sets the UID of the current user.
		/// </summary>
		/// <param name="uid">The UID of the current user.</param>
		public void SetCurrentUserUid(string uid)
		{
			_currentUserUid = uid;
		}

		/// <summary>
		/// Sign Out Method
		/// </summary>
		/// <returns></returns>
		public async Task<bool> SignOutAsync()
		{
			try
			{
				await Application.Current.MainPage.DisplayAlert("Confirmation", "You have logged out", "Ok");

				_currentUserUid = null;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
