using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blaz_AppForTest.Pages
{
	public partial class Index
	{
		/// <summary>
		/// Inject the Login Processor
		/// </summary>
		[Inject]
		private ILoginProcessor loginProcessor { get; set; }
		private LoginModel login= new();
		private string errorMessage = string.Empty;

		private bool ValidateEmail()
		{
			return !string.IsNullOrEmpty(login.Email);
		}

		private bool ValidatePassword()
		{
			return !string.IsNullOrEmpty(login.Password);
		}

		public async Task Login()
		{
			if (ValidateEmail() && ValidatePassword())
			{
				if (!loginProcessor.Login(login.Email, login.Password))
				{
					errorMessage = "Invalid Login";
				}
			}
			else 
			{
				errorMessage = "Email/Password Invalid";
			}
		}

	}
}
