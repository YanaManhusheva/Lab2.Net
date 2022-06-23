using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.LINQ.GeneralData
{
    public class Data
    {
        public IEnumerable<Employee> Employees { get; init; }   
        public IEnumerable<Owner> Owners { get; set; }
        public IEnumerable<Project> Projects { get; init; }
        public IEnumerable<ProjectEmployee> ProjectsEmployees { get; init; }
      

    }
}
