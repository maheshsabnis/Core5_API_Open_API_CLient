using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBaseAuthentication.Models;
using TokenBaseAuthentication.Services;

namespace TokenBaseAuthentication.Controllers
{
	[Route("api/[controller]")]
	[AllowAnonymous]
	[ApiController]
	public class AuthController : ControllerBase
	{
        CoreAuthService authenticationService;
        public AuthController(CoreAuthService authenticationService)
        {
            this.authenticationService = authenticationService;
        }


        [HttpPost("/register")]
        public async Task<ResponseData> Register(RegisterUser user)
        {
            if (ModelState.IsValid)
            {
                var IsCreated = await authenticationService.RegisterUserAsync(user);
                if (IsCreated == false)
                {
                    return new ResponseData() { Message = "The User Already Present" };
                }
                var ResponseData = new ResponseData()
                {
                    Message = $"{user.Email} User Created Successfully"
                };
                return ResponseData;
            }
           
            return  new ResponseData() { Message = "Error in Model" };
        }

        [HttpPost("/auth")]
        public async Task<ResponseData> Login(LoginUser inputModel)
        {
            if (ModelState.IsValid)
            {
                var token = await authenticationService.AuthenticateUserAsync(inputModel);
                if (token == null)
                {
                    return new ResponseData() { Message = "Authentiucation Failed" };
                }
                var ResponseData = new ResponseData()
                {
                    Message = token
                };

                return ResponseData;
            }
            return new ResponseData() { Message = "Model Error Occured" };
        }

    }
}
