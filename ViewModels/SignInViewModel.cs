using Firebase.Auth;
using Firebase.Auth.Providers;
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

			try
			{
				var user = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

				if (user != null)
				{
					string userUid = user.User.Uid;
					Debug.WriteLine($"User signed in successfully: {Email}, UID: {userUid}");
					UserService.Instance.SetCurrentUserUid(userUid);

					await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
				}
			}
			catch (FirebaseAuthException ex)
			{
				Debug.WriteLine($"Error during sign in: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign in", "OK");
			}

			Debug.WriteLine("ExecuteSignInAsync method completed.");
		}

		private async Task ExecuteSignUpAsync()
		{
			await Shell.Current.GoToAsync($"//SignUp");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}


