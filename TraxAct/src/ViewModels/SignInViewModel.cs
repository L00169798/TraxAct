using Firebase.Auth;
using Firebase.Auth.Providers;
using System.ComponentModel;
using System.Windows.Input;
using TraxAct.Services;
using TraxAct.Views;

namespace TraxAct.ViewModels
{
	public class SignInViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseAuthClient _authClient;

		/// <summary>
		/// Constructor
		/// </summary>
		public SignInViewModel()
		{
			var authConfig = new FirebaseAuthConfig
			{
				//Register Firebase Authentication
				ApiKey = "AIzaSyBCmctzgS7IOUNUKnorKAEpezbSaWrRL_Y",
				AuthDomain = "localhost",
				Providers = new FirebaseAuthProvider[] { new EmailProvider() } 
			};

			_authClient = new FirebaseAuthClient(authConfig);

			ClearForm();

			SignInCommand = new Command(async () => await ExecuteSignInAsync());
			SignUpCommand = new Command(async () => await ExecuteSignUpAsync());
		}

		/// <summary>
		/// Properties
		/// </summary>
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

		public ICommand SignInCommand { get; }
		public ICommand SignUpCommand { get; }

		/// <summary>
		/// SignIn Method
		/// </summary>
		/// <returns></returns>
		private async Task ExecuteSignInAsync()
		{
			if (!IsValidEmail(Email))
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Invalid email format. Please enter a valid email address.", "OK");
				return;
			}

			try
			{
				var user = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

				if (user != null)
				{
					string userUid = user.User.Uid;
					UserService.Instance.SetCurrentUserUid(userUid);

					//Navigate to homepage after signing in
					await Shell.Current.GoToAsync($"///{nameof(HomePage)}");

					ClearForm();
				}
			}
			catch (FirebaseAuthException ex)
			{
				if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials. Please check your credentials or sign up.", "OK");
				}
				else if (ex.Message.Contains("MissingPassword"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Please enter password", "OK");
				}
				else if (ex.Message.Contains("TOO_MANY_ATTEMPTS"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Incorrect credentials entered too many times, please try again later", "OK");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign in. Please try again later.", "OK");
				}
			}
			catch
			{
				await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
			}
		}

		/// <summary>
		/// Method to validate email format
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Method to execute sign in form
		/// </summary>
		/// <returns></returns>
		private async Task ExecuteSignUpAsync()
		{
			await Shell.Current.GoToAsync($"//SignUp");
		}

		/// <summary>
		/// Method to clear sign in form
		/// </summary>
		private void ClearForm()
		{
			Email = string.Empty;
			Password = string.Empty;
		}

		/// <summary>
		/// Method to handle property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}


