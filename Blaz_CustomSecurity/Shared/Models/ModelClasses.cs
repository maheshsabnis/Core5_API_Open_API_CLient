using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaz_CustomSecurity.Shared.Models
{
	public class RegisterUser
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
	}

	public class AuthUser
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class CurrentUser
	{
		public bool IsAuthenticated { get; set; }
		public string Email { get; set; }
		public Dictionary<string, string> Claims { get; set; }
	}



}
