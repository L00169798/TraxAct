using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraxAct.Models;
using Microsoft.EntityFrameworkCore;

namespace TraxAct.Services
{
	public class UserService
	{
		private static UserService _instance;
		private string _currentUserUid;
		private readonly MyDbContext _dbContext;

		public static UserService Instance => _instance ??= new UserService();


		public string GetCurrentUserUid()
		{
			return _currentUserUid;
		}

		public void SetCurrentUserUid(string uid)
		{
			_currentUserUid = uid;
		}


		public bool SignOut()
		{
			try
			{
				_currentUserUid = null;
				Console.WriteLine("User signed out successfully.");
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during sign out: {ex.Message}");
				return false; 
			}
		}
	}
}

