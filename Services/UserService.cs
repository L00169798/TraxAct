using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraxAct.Services
{
	public class UserService
	{
		private static UserService _instance;
		private string _currentUserUid;

		public static UserService Instance => _instance ??= new UserService();

		public string GetCurrentUserUid()
		{
			return _currentUserUid;
		}

		public void SetCurrentUserUid(string uid)
		{
			_currentUserUid = uid;
		}
	}
}

