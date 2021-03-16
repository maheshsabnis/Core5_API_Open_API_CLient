using Core5_API.Models;
using Core5_API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core5_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Consumes(MediaTypeNames.Application.Json)]
	[ApiVersion("1.0")]
	[ApiVersion("1.1")]
	public class DepartmentController : ControllerBase
	{
		private readonly IService<Department, int> deptServ;

		public DepartmentController(IService<Department, int> serv)
		{
			deptServ = serv;
		}

		// define an operationId
		//[HttpGet("/departments",Name =nameof(Get))]
		// GET: api/<Department>
		[HttpGet("/departments/getall")]
		//public async Task<IActionResult> Get()
		public async Task<IEnumerable<Department>>  Get()
		{
			var result = await deptServ.GetAsync();
			return result;
		}


		// GET api/<Department>/5
		[HttpGet("/departments/getone/{id}")]
		public async Task<Department> Get(int id)
		{
			var result = await deptServ.GetAsync(id);
			return  result;
		}

		// POST api/<Department>
		[HttpPost("/department/createone")]
		public async Task<Department> Post([FromBody] Department dept)
		{
			if (ModelState.IsValid)
			{
				var result = await deptServ.CreateAsync(dept);
				return  result;
			}
			return dept;
			
		}

		// PUT api/<Department>/5
		[HttpPut("/department/updateone/{id}")]
		public async Task<Department> Put(int id, [FromBody] Department dept)
		{
			if (ModelState.IsValid)
			{
				var result = await deptServ.UpdateAsync(id,dept);
				return result;
			}
			return dept;
		}

		// DELETE api/<Department>/5
		[HttpDelete("/departments/deleteone/{id}")]
		public async Task<bool> Delete(int id)
		{
			var result = await deptServ.DeleteAsync(id);
			return result;
		}
	}
}
