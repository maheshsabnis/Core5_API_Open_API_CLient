using Core5_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core5_API.Services
{
	public class EmployeeService : IService<Employee, int>
	{
		private readonly CompanyContext context;
		public EmployeeService(CompanyContext context)
		{
			this.context = context;
		}

		public async Task<Employee> CreateAsync(Employee entity)
		{
			var result = await context.Employees.AddAsync(entity);
			await context.SaveChangesAsync();
			return result.Entity;

		}

		public async Task<bool> DeleteAsync(int id)
		{
			var dept = await context.Employees.FindAsync(id);
			if (dept == null) return false;
			context.Remove(dept);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<List<Employee>> GetAsync()
		{
			return await context.Employees.ToListAsync();
		}

		public async Task<Employee> GetAsync(int id)
		{
			var dept = await context.Employees.FindAsync(id);
			return dept;
		}

		public async Task<Employee> UpdateAsync(int id, Employee entity)
		{
			var dept = await context.Employees.FindAsync(id);
			if (dept == null) return null;

			context.Update<Employee>(entity);
			await context.SaveChangesAsync();

			return dept;
		}
	}
}
