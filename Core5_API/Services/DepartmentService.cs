using Core5_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core5_API.Services
{
	public class DepartmentService : IService<Department, int>
	{
		private readonly CompanyContext context;
		public DepartmentService(CompanyContext context)
		{
			this.context = context;
		}

		public async Task<Department> CreateAsync(Department entity)
		{
			var result = await context.Departments.AddAsync(entity);
			await context.SaveChangesAsync();
			return result.Entity;

		}

		public async Task<bool> DeleteAsync(int id)
		{
			var dept = await context.Departments.FindAsync(id);
			if (dept == null) return false;
			context.Remove(dept);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<List<Department>> GetAsync()
		{
			return await context.Departments.ToListAsync();
		}

		public async Task<Department> GetAsync(int id)
		{
			var dept = await context.Departments.FindAsync(id);
			return dept;
		}

		public async Task<Department> UpdateAsync(int id, Department entity)
		{
			var dept = await context.Departments.FindAsync(id);
			if (dept == null) return null;

			context.Update<Department>(entity);
			await context.SaveChangesAsync();

			return dept;
		}
	}
}
