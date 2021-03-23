using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrwk_Library
{
    public class ClsData
    {
        public List<Employee> GetEmployees()
        {
            return new Employees();
        }
        public int Add(int x,int y)
        {
            return x + y;
        }
    }
}
