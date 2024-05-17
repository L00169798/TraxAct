using Firebase.Auth;
using Firebase.Auth.Providers;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class SignUpViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseAuthClient _authClient;

		/// <summary>
		/// Constructor
		/// </summary>
		public SignUpViewModel()
		{
			//Configure Firebase authentication
			var authConfig = new FirebaseAuthConfig
			{
				ApiKey = "AIzaSyBCmctzgS7IOUNUKnorKAEpezbSaWrRL_Y",
				AuthDomain = "localhost",
				Providers = new FirebaseAuthProvider[] { new EmailProvider() }
			};

			_authClient = new FirebaseAuthClient(authConfig);

			//Inititalise commands
			SignUpCommand = new Command(async () => await ExecuteSignUpAsync());
			SignInCommand = new Command(async () => await ExecuteSignInAsync());
		}

		//Properties
		private string _email;
		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		private string _confirmPassword;
		public string ConfirmPassword
		{
			get { return _confirmPassword; }
			set
			{
				_confirmPassword = value;
				OnPropertyChanged(nameof(ConfirmPassword));
			}
		}

		// Commands
		public ICommand SignUpCommand { get; }
		public ICommand SignInCommand { get; }


		/// <summary>
		/// Sign Up method
		/// </summary>
		/// <returns></returns>
		private async Task ExecuteSignUpAsync()
		{
			if (!IsPasswordValid(Password))
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Password does not meet requirements", "OK");
				return;
			}

			try
			{
				//Pasword validation
				if (Password != ConfirmPassword)
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
					return;
				}

				// Create user with email and password
				var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);

				if (userCredential?.User != null && !string.IsNullOrEmpty(userCredential.User.Uid))
				{ //Save User Id to database after sign up
					await SaveUserIdAsync(userCredential.User.Uid);

					await Application.Current.MainPage.DisplayAlert("Welcome", "Registration Successful!", "OK");

					await Shell.Current.GoToAsync("//SignIn");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
				}
			}
			catch (Firebase.Auth.FirebaseAuthException ex)
			{
				if (ex.Message.Contains("EmailExists"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Account already exists, return to sign in page", "OK");
				}
				else if (ex.Message.Contains("MissingPassword"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Please enter password", "OK");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
				}
			}
			catch
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
			}
		}


		/// <summary>
		/// Sign In method
		/// </summary>
		/// <returns></returns>
		private static async Task ExecuteSignInAsync()
		{
			try
			{
				await Shell.Current.GoToAsync("//SignIn");
			}
			catch
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to navigate to sign-in page", "OK");
			}
		}

		/// <summary>
		/// Save user Id to database on sign up
		/// </summary>
		/// <param name="firebaseUid"></param>
		/// <returns></returns>
		private static async Task SaveUserIdAsync(string firebaseUid)
		{
			await SaveUserIdToDbContextAsync(firebaseUid);
		}

		private static async Task SaveUserIdToDbContextAsync(string firebaseUid)
		{
			var dbContext = new MyDbContext();
			await dbContext.SaveUserId(firebaseUid);
		}

		/// <summary>
		/// Password validity parameters
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		private bool IsPasswordValid(string password)
		{
			const string regexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

			return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, regexPattern);
		}

		/// <summary>
		///  On property changed event handler
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}