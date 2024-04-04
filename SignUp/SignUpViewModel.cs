using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TraxAct.ViewModels;

namespace TraxAct.SignUp
{
	public class SignUpViewModel : ViewModelBase
	{
		private string _email;
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}

		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}

		}

		private string _confirmPassword;
		public string ConfirmPassword
		{
			get
			{
				return _confirmPassword;
			}
			set
			{
				_confirmPassword = value;
				OnPropertyChanged(nameof(ConfirmPassword));
			}

		}

		public ICommand SignUpCommand { get; }

		public SignUpViewModel(FirebaseAuthClient authClient)
		{
			SignUpCommand = new Command(async () => await ExecuteSignUpAsync(authClient));
		}

		private async Task ExecuteSignUpAsync(FirebaseAuthClient authClient)
		{
			var signUpCommand = new SignUpCommand(this, authClient);
			await signUpCommand.ExecuteAsync(null);
		}


	}
}
