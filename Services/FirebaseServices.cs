using System.Threading.Tasks;
using Firebase.Auth;
using FirebaseAdmin.Auth;

public class FirebaseServices
{
	private readonly FirebaseAuth _firebaseAuth;

	public FirebaseServices()
	{
		_firebaseAuth = FirebaseAuth.DefaultInstance;
	}

	public async Task<UserRecord> GetUserByUidAsync(string uid)
	{
		try
		{
			return await _firebaseAuth.GetUserAsync(uid);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching user by UID: {ex.Message}");
			return null;
		}
	}

	public async Task<UserRecord> GetUserByEmailAsync(string email)
	{
		try
		{
			return await _firebaseAuth.GetUserByEmailAsync(email);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching user by email: {ex.Message}");
			return null;
		}
	}

	public async Task<UserRecord> GetUserByPhoneNumberAsync(string phoneNumber)
	{
		try
		{
			return await _firebaseAuth.GetUserByPhoneNumberAsync(phoneNumber);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching user by phone number: {ex.Message}");
			return null;
		}
	}
}
