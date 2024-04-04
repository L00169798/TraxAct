using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraxAct.SignUp
{
	public class SignUpCommand 
	{

		private readonly SignUpViewModel _viewModel;
		private readonly FirebaseAuthClient _authClient;

		public SignUpCommand(SignUpViewModel viewModel, FirebaseAuthClient authClient)
		{
			_viewModel = viewModel;
			_authClient = authClient;
		}

		public async Task ExecuteAsync(object parameter)
		{
			if (_viewModel.Password != _viewModel.ConfirmPassword)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
				return; 
			}
			try
			{

				await _authClient.CreateUserWithEmailAndPasswordAsync(_viewModel.Email, _viewModel.Password);
				await Application.Current.MainPage.DisplayAlert("Success", "Successfully signed up", "OK");
			}
			catch (Exception)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign up", "please try again later");
			}

			}
		}
	}

