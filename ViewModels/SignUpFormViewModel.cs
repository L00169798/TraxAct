using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraxAct.SignUp;

namespace TraxAct.ViewModels
{
	public class SignUpFormViewModel
	{
		public SignUpViewModel SignUpViewModel { get; }
		public SignUpFormViewModel(SignUpViewModel signUpViewModel)
		{
			SignUpViewModel = signUpViewModel;
		}
	}
}

