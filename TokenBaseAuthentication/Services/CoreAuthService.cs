using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenBaseAuthentication.Models;

namespace TokenBaseAuthentication.Services
{
    /// <summary>
    /// The followint class is used for the following
    /// 1. The class will accept Login Details and then Issue the token based on the login Status
    /// 
    /// </summary>
    public class CoreAuthService
    {
        IConfiguration configuration;
        SignInManager<IdentityUser> signInManager;
        UserManager<IdentityUser> userManager;
        /// <summary>
        /// 1.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public CoreAuthService(IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// 2.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task<bool> RegisterUserAsync(RegisterUser register)
        {
            bool IsCreated = false;
            var registerUser = new IdentityUser() { UserName = register.Email, Email = register.Email };
            // 2a.
            var result = await userManager.CreateAsync(registerUser, register.Password);
            if (result.Succeeded)
            {
                IsCreated = true;
            }
            return IsCreated;
        }
        /// <summary>
        /// 3.
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public async Task<string> AuthenticateUserAsync(LoginUser inputModel)
        {

            string jwtToken = "";

            // 3a.
            var result = await signInManager.PasswordSignInAsync(inputModel.UserName, inputModel.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                //3b Read the secret key and the expiration from the configuration 
                var secretKey = Convert.FromBase64String(configuration["JWTCoreSettings:SecretKey"]);
                var expiryTimeSpan = Convert.ToInt32(configuration["JWTCoreSettings:ExpiryInMinuts"]);
                //3c. logic to get the user role
                // get the user object based on Email

                IdentityUser user = new IdentityUser(inputModel.UserName);

                //3d set the expiry, subject, etc.
                // note that Issuer and Audience will be null because 
                // there is no third-party issuer
                var securityTokenDescription = new SecurityTokenDescriptor()
                {
                    Issuer = null,
                    Audience = null,
                    Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("username",user.Id,ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expiryTimeSpan),
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };
                //3e Now generate token using JwtSecurityTokenHandler
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwToken = jwtHandler.CreateJwtSecurityToken(securityTokenDescription);
                jwtToken = jwtHandler.WriteToken(jwToken);
            }
            else
            {
                jwtToken = "Login failed";
            }

            return jwtToken;
        }
    }
}
