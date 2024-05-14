using FirebaseAdmin.Auth;

namespace TraxAct.Services
{
	/// <summary>
	/// Provides services for interacting with Firebase Authentication.
	/// </summary>
	public class FirebaseServices
	{
		private readonly FirebaseAuth _firebaseAuth;

		/// <summary>
		/// Initializes a new instance of the <see cref="FirebaseServices"/> class.
		/// </summary>
		public FirebaseServices()
		{
			_firebaseAuth = FirebaseAuth.DefaultInstance;
		}

		/// <summary>
		/// Retrieves a user record based on the specified UID asynchronously.
		/// </summary>
		/// <param name="uid">The UID of the user.</param>
		/// <returns>A <see cref="UserRecord"/> object representing the user with the specified UID.</returns>
		public async Task<UserRecord> GetUserByUidAsync(string uid)
		{
			try
			{
				return await _firebaseAuth.GetUserAsync(uid);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Retrieves a user record based on the specified email asynchronously.
		/// </summary>
		/// <param name="email">The email address of the user.</param>
		/// <returns>A <see cref="UserRecord"/> object representing the user with the specified email.</returns>
		public async Task<UserRecord> GetUserByEmailAsync(string email)
		{
			try
			{
				return await _firebaseAuth.GetUserByEmailAsync(email);
			}
			catch 
			{
				return null;
			}
		}
	}
}
