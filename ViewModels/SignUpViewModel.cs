using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TraxAct.Views;
using TraxAct.Services;
using FirebaseAdmin.Auth;

namespace TraxAct.ViewModels
{
	public class SignUpViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseAuthClient _authClient;

		public SignUpViewModel()
		{
			var authConfig = new FirebaseAuthConfig
			{
				ApiKey = "AIzaSyBCmctzgS7IOUNUKnorKAEpezbSaWrRL_Y",
				AuthDomain = "localhost",
				Providers = new FirebaseAuthProvider[] { new EmailProvider() }
			};

			_authClient = new FirebaseAuthClient(authConfig);

			SignUpCommand = new Command(async () => await ExecuteSignUpAsync());
			SignInCommand = new Command(async () => await ExecuteSignInAsync());
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

		public ICommand SignUpCommand { get; }
		public ICommand SignInCommand { get; }

		private async Task ExecuteSignUpAsync()
		{
			Debug.WriteLine("ExecuteSignUpAsync method started.");

			try
			{
				if (Password != ConfirmPassword)
				{
					Debug.WriteLine("Passwords do not match.");
					await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
					return;
				}

				var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);

				if (userCredential?.User != null && !string.IsNullOrEmpty(userCredential.User.Uid))
				{
					SaveUserId(userCredential.User.Uid);

					await Application.Current.MainPage.DisplayAlert("Welcome", "Registration Successful!", "OK");

					await Shell.Current.GoToAsync("//SignIn");
				}
				else
				{
					Debug.WriteLine("User or UID is null.");
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
				}
			}
			catch (Firebase.Auth.FirebaseAuthException ex)
			{
				Debug.WriteLine($"Firebase authentication error: {ex.Message}");

				if (ex.Message.Contains("EmailExists"))
				{
					Debug.WriteLine("Account already exists");
					await Application.Current.MainPage.DisplayAlert("Error", "Account already exists, return to sign in page", "OK");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error during sign up: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
			}

			Debug.WriteLine("ExecuteSignUpAsync method completed.");
		}

		private async Task ExecuteSignInAsync()
		{
			try
			{
				await Shell.Current.GoToAsync("//SignIn");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error navigating to sign-in page: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to navigate to sign-in page", "OK");
			}
		}


		private void SaveUserId(string firebaseUid)
		{
			try
			{
				var dbContext = new MyDbContext();
				dbContext.SaveUserId(firebaseUid);
				Debug.WriteLine("Firebase UID saved to SQLite database successfully.");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error saving Firebase UID to SQLite database: {ex.Message}");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}