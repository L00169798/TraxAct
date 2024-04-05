using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TraxAct.Views;

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

				var user = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);

				await Application.Current.MainPage.DisplayAlert("Success", "Successfully signed up", "OK");
				await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error during sign up: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up. Please try again later", "OK");
			}

			Debug.WriteLine("ExecuteSignUpAsync method completed.");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
