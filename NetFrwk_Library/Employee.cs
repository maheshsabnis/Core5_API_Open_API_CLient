using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrwk_Library
{
	public class Employee
	{
		public int EmpNo { get; set; }
		public string EmpName { get; set; }
	}

	public class Employees : List<Employee>
	{
		public Employees()
		{
			Add(new Employee() { EmpNo = 101, EmpName = "A" });
			Add(new Employee() { EmpNo = 102, EmpName = "B" });
		}
	}

}
