using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TraxAct.Views;
using TraxAct.Services;

namespace TraxAct.ViewModels
{
	public class SignInViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseAuthClient _authClient;

		public SignInViewModel()
		{
			var authConfig = new FirebaseAuthConfig
			{

				ApiKey = "AIzaSyBCmctzgS7IOUNUKnorKAEpezbSaWrRL_Y",
				AuthDomain = "localhost",
				Providers = new FirebaseAuthProvider[] { new EmailProvider() }
			};

			_authClient = new FirebaseAuthClient(authConfig);

			ClearForm();

			SignInCommand = new Command(async () => await ExecuteSignInAsync());
			SignUpCommand = new Command(async () => await ExecuteSignUpAsync());
		}

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

		private async Task ExecuteSignInAsync()
		{
			Debug.WriteLine("ExecuteSignInAsync method started.");

			
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
					Debug.WriteLine($"User signed in successfully: {Email}, UID: {userUid}");
					UserService.Instance.SetCurrentUserUid(userUid);

					Debug.WriteLine($"Current User ID: {UserService.Instance.GetCurrentUserUid()}");

					await Shell.Current.GoToAsync($"///{nameof(HomePage)}");

					ClearForm();
				}
			}
			catch (FirebaseAuthException ex)
			{
				Debug.WriteLine($"Error during sign in: {ex.Message}");

				if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials. Please check your credentials or sign up.", "OK");
				}
				else if (ex.Message.Contains("MissingPassword"))
				{
					Debug.WriteLine("MissingPassword");
					await Application.Current.MainPage.DisplayAlert("Error", "Please enter password", "OK");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign in. Please try again later.", "OK");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unexpected error during sign in: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
			}

			Debug.WriteLine("ExecuteSignInAsync method completed.");
		}

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



		private async Task ExecuteSignUpAsync()
		{
			await Shell.Current.GoToAsync($"//SignUp");
		}

		private void ClearForm()
		{
			Email = string.Empty;
			Password = string.Empty;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}


