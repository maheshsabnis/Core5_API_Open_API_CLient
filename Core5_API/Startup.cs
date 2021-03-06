using Core5_API.Models;
using Core5_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Core5_API
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

			services.AddDbContext<CompanyContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("DbConnStr"));
			});
			services.AddScoped<IService<Department, int>, DepartmentService>();
			services.AddScoped<IService<Employee, int>, EmployeeService>();

			services.AddControllers().AddJsonOptions(options=> {

				options.JsonSerializerOptions.PropertyNamingPolicy = null;
				}).AddXmlDataContractSerializerFormatters();

			services.AddApiVersioning(
				  options => {options.DefaultApiVersion = ApiVersion.Parse("1.0");
					  options.AssumeDefaultVersionWhenUnspecified = true;
				  }
				) ;
			;

			services.AddCors(options=> {
				options.AddPolicy("corspolicy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});
			//services.AddVersionedApiExplorer(options =>
			//{
			//	options.DefaultApiVersion = ApiVersion.Parse("1.0");
			//	options.AssumeDefaultVersionWhenUnspecified = true;
			//});


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core5_API", Version = "v1" });
			});

			 
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				 app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core5_API v1"));
				 
			}

			app.UseHttpsRedirection();
			app.UseCors("corspolicy");
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
