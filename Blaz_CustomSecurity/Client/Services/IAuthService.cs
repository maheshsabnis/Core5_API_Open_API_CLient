using Blaz_CustomSecurity.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blaz_CustomSecurity.Client.Services
{
	public interface IAuthService
	{
		Task Login(AuthUser loginRequest);
		Task Register(RegisterUser registerRequest);
		Task Logout();
		Task<CurrentUser> CurrentUserInfo();
	}
}
