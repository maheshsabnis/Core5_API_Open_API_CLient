using Balzor_App_Rbs.Server.Data;
using Balzor_App_Rbs.Server.Models;
using Balzor_App_Rbs.Server.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Balzor_App_Rbs.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			services.AddDatabaseDeveloperPageExceptionFilter();

			//services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
			//	.AddEntityFrameworkStores<ApplicationDbContext>();

			//services.AddIdentityServer()
			//	.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddIdentity<IdentityUser, IdentityRole>()
				   .AddEntityFrameworkStores<ApplicationDbContext>()
			   .AddDefaultTokenProviders();

			services.AddCors(options =>
			{
				options.AddPolicy("corspolicy", (policy) =>
				{
					policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});


			services.AddAuthentication()
				.AddIdentityServerJwt();

			services.AddScoped<AuthSecurityService>();

			services.AddControllersWithViews();
			services.AddRazorPages();

			services.AddAuthorization(options =>
			{
				options.AddPolicy("AdminPolicy", (policy) =>
				{
					policy.RequireRole("Administrator");
				});

				options.AddPolicy("AdminManagerPolicy", (policy) =>
				{
					policy.RequireRole("Administrator", "Manager");
				});

				options.AddPolicy("AdminManagerClerkPolicy", (policy) =>
				{
					policy.RequireRole("Administrator", "Manager", "Clerk");
				});
			});


			byte[] secretKey = Convert.FromBase64String(Configuration["JWTCoreSettings:SecretKey"]);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(secretKey),
					ValidateIssuer = false,
					ValidateAudience = false
				};
				x.Events = new JwtBearerEvents()
				{
					OnAuthenticationFailed = context =>
					{
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("Authentication-Token-Expired", "true");
						}
						return Task.CompletedTask;
					}
				};
			}).AddCookie(options =>
			{
				options.Events.OnRedirectToAccessDenied =
				options.Events.OnRedirectToLogin = c =>
				{
					c.Response.StatusCode = StatusCodes.Status401Unauthorized;
					return Task.FromResult<object>(null);
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  IServiceProvider serv)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseIdentityServer();
			app.UseCors("corspolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			// create the default application adminstrator
			CreateApplicationAdministrator(serv).Wait();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}

		private async Task CreateApplicationAdministrator(IServiceProvider serviceProvider)
		{
			try
			{
				var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

				IdentityResult result;
				// add a new Administrator role for the application
				var isRoleExist = await roleManager.RoleExistsAsync("Administrator");
				if (!isRoleExist)
				{
					// create Administrator Role and add it in Database
					result = await roleManager.CreateAsync(new IdentityRole("Administrator"));
				}

				// code to create a default user and add it to Administrator Role
				var user = await userManager.FindByEmailAsync("mahesh@myapp.com");
				if (user == null)
				{
					var defaultUser = new IdentityUser() { UserName = "mahesh@myapp.com", Email = "mahesh@myapp.com" };
					var regUser = await userManager.CreateAsync(defaultUser, "P@ssw0rd_");
					await userManager.AddToRoleAsync(defaultUser, "Administrator");
				}
			}
			catch (Exception ex)
			{
				var str = ex.Message;
			}

		}
	}
}
