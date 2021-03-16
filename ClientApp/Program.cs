using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenAPIClient;
namespace ClientApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var httpClient = new HttpClient();
			 
		 	 var apiClient = new DepartmentAPIClient("https://localhost:44325/", httpClient);

			  var res =  await apiClient.GetallAsync(System.Threading.CancellationToken.None);
			 
			 Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(res));

			Department dept = new Department()
			{
				DeptNo = 1001, DeptName = "Dept_1001", Location = "Pune"
			};

			var result = await apiClient.CreateoneAsync(dept);
			Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
			Console.WriteLine("Hello World!");
			Console.ReadLine();
		}
	}
}
