using System;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using TraxAct.Models; 
using TraxAct.Services;
using TraxAct.ViewModels;
using Xunit;

namespace TraxActUnitTests.UnitTests
{
	public class AppFunctionalityTests
	{
		private Mock<IAuthService> _authServiceMock;

		public AppFunctionalityTests()
		{
			_authServiceMock = new Mock<IAuthService>();
		}

		[Fact]
		public async Task Login_ValidCredentials_ShouldSucceed()
		{
			// Arrange
			var username = "test_user";
			var password = "secure_password";
			_authServiceMock.Setup(x => x.Login(username, password)).Returns(Task.FromResult(true));

			var signInViewModel = new SignInViewModel(_authServiceMock.Object);

			// Act
			signInViewModel.Email = username;
			signInViewModel.Password = password;
			await Task.Run(() => signInViewModel.SignInCommand.Execute());

			// Assert
			Assert.True(/* Assuming a way to check login state */);
			_authServiceMock.Verify(x => x.Login(username, password), Times.Once);
		}

		[Fact]
		public async Task Login_InvalidCredentials_ShouldFail() 
		{
			// Arrange
			var username = "invalid_user";
			var password = "wrong_password";
			_authServiceMock.Setup(x => x.Login(username, password)).Returns(Task.FromResult(false)); 

			var loginViewModel = new SignInViewModel(_authServiceMock.Object); 

			// Act
			loginViewModel.Username = username;
			loginViewModel.Password = password;
			var loginSuccess = await loginViewModel.LoginCommand.Execute(); 

			// Assert
			Assert.False(loginSuccess);
			_authServiceMock.Verify(x => x.Login(username, password), Times.Once);
		}
	}
}