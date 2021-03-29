using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBaseAuthentication.Models;
using TokenBaseAuthentication.Services;

namespace TokenBaseAuthentication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{


			services.AddDbContext<SecurityDbContext>(options=> {
				options.UseSqlServer(Configuration.GetConnectionString("SecurityDbContextConnection"));
			});


			services.AddDbContext<MyAppDbContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("DataAppContextConnection"));
			});


			// ASP.NET Core 5 Identity Registration
			// Resolve UserManager and ServiceManager Classes using IdentityBuilder
			services.AddIdentity<IdentityUser,IdentityRole>(
			   options => options.SignIn.RequireConfirmedAccount = false)
			   .AddEntityFrameworkStores<SecurityDbContext>();


			services.AddScoped<CoreAuthService>();
			// use the secret key this will be used for integrity check for the received token 
			byte[] secretKey = Convert.FromBase64String(Configuration["JWTCoreSettings:SecretKey"]);

			// this will read the request headerb and check is the Auth heade present
			// in incomming request
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
		.AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			// validate the token using the key 
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(secretKey),
				ValidateIssuer = false,
				ValidateAudience = false
			};
		});

			services.AddCors(options=> {
				options.AddPolicy("corspolicy",
					  policy=>policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			services.AddScoped<IRepository<Product, int>, ProductRepository>();
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenBaseAuthentication", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenBaseAuthentication v1"));
			}
			app.UseCors("corspolicy");
			app.UseRouting();
			// Middlewares those will be depend upon the
			// Authentication service for
			// User Based Authentication
			// and then based on JWT Tokens
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
