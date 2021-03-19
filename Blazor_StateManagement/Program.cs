using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazor_StateManagement.StateService;

namespace Blazor_StateManagement
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			// adding the localStorage for the WebAssembly App
			// A library to provide access to local storage in Blazor applications
			builder.Services.AddBlazoredLocalStorage();
			// a servic for the SessionStorage
			builder.Services.AddBlazoredSessionStorage();

			builder.Services.AddSingleton<StateContainerService>();

			await builder.Build().RunAsync();
		}
	}
}
