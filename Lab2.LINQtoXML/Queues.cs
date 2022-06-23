using Lab1.LINQ.GeneralData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Lab2.LINQtoXML
{
    public class Queues
    {
        private const string _employeesFile = "employees.xml";
        private const string _ownersFile = "owners.xml";
        private const string _projectsFile = "projects.xml";
        private const string _projectsEmployeesFile = "projectsEmployees.xml";

        private  XElement _employees;
        private  XElement _owners;
        private  XElement _projects;
        private  XElement _projectsEmployees;

        public Queues()
        {
            UpdateXEmployees();
            UpdateXOwners();
            UpdateXProjects();
            UpdateXProjectsEmployees();
        }

        public void UpdateXEmployees()
        {
            var EmployeesDoc = XDocument.Load(_employeesFile);
            _employees = EmployeesDoc.Element("employees");

        }
        public void UpdateXOwners()
        {
            var OwnersDoc = XDocument.Load(_ownersFile);
            _owners = OwnersDoc.Element("owners");
        }
        public void UpdateXProjects()
        {
            var ProjectsDoc = XDocument.Load(_projectsFile);
            _projects = ProjectsDoc.Element("projects");
        }
        public void UpdateXProjectsEmployees()
        {
            var ProjectsDoc = XDocument.Load(_projectsEmployeesFile);
            _projectsEmployees = ProjectsDoc.Element("projectsEmployees");
        }



        /// <summary>
        /// 1
        /// Joining owners of the project with their project and then grouping by projects Name
        /// </summary>
        public IOrderedEnumerable<QueriesPrinter> JoinOwnerProject()
        {
            var joinOP = _projects.Elements("project").GroupJoin(_owners.Elements("owner"),
                p => p.Element("code").Value,
                o => o.Element("project").Element("code").Value,
                (p, owners) => new QueriesPrinter { 
                    FieldOne = p.Element("name").Value, 
                    FieldTwo = owners.Select(o=> o.Element("name").Value + " "+ o.Element("surname").Value)
                    .Aggregate((s1, s2) => s1 + "\n" + s2)
                }).OrderBy(p => p.FieldOne);

            return joinOP;
        }
      
        /// <summary>
        /// 2
        /// Finding Employyes who work on both projects
        /// </summary>
        public IEnumerable<QueriesPrinter> IntersectProjectsEmployees()
        {
            var intersectPE = _projectsEmployees.Elements("projectEmployee")
                .Where(p => p.Element("project").Element("code").Value ==
                    _projects.Elements("project").ElementAt(0).Element("code").Value)
                .Select(p => new
                {
                    Name = p.Element("employee").Element("name").Value,
                    Surname = p.Element("employee").Element("surname").Value
                })
                .Intersect(_projectsEmployees.Elements("projectEmployee")
                    .Where(p => p.Element("project").Element("code").Value == 
                         _projects.Elements("project").ElementAt(1).Element("code").Value)
                     .Select(p => new
                     {
                         Name = p.Element("employee").Element("name").Value,
                         Surname = p.Element("employee").Element("surname").Value
                     }))
                .Select(p => new QueriesPrinter
                 {
                     FieldOne = p.Name,
                     FieldTwo = p.Surname
                 });

            return intersectPE;


        }
        /// <summary>
        /// 3
        /// Finding the duration of finished projects and sorting them by EndDate
        /// </summary>
        public IEnumerable<QueriesPrinter> FilterDateProjects()
        {
            var filterDP = from project in _projects.Elements("project")
                           where DateTime.Parse(project.Element("endDate").Value) <= DateTime.Now
                           orderby DateTime.Parse(project.Element("endDate").Value)
                           select new QueriesPrinter
                           {
                               FieldOne = project.Element("name").Value,
                               FieldTwo = (DateTime.Parse(project.Element("endDate").Value) - 
                               DateTime.Parse(project.Element("startDate").Value)).TotalDays.ToString()
                           };
            return filterDP;

        }

        /// <summary>
        /// 4
        /// Finding the most popular position among Projects
        /// </summary>
        public QueriesPrinter MostPopularPosition()
        {

            var mostPopularPosition = _projectsEmployees.Elements("projectEmployee")
                .Select(p => p.Element("employee").Element("position").Value)
                .GroupBy(p => p)
                .Select(p => new { Position = p.Key, PeopleCount = p.Count() });

         
            var maxVal = mostPopularPosition.Max(p => p.PeopleCount);
            var popularPos = mostPopularPosition.First(p => p.PeopleCount == maxVal);

            return new QueriesPrinter
            {
                FieldOne = popularPos.Position,
                FieldTwo = popularPos.PeopleCount.ToString()
            };
        }
       
        /// <summary>
        /// 5
        /// Finding the amount of workers on each project and grouping by the number of employees
        /// </summary>
        public IEnumerable<QueriesPrinter> NumberEmployees()
        {
            var numberOfEmployees = _projectsEmployees.Elements("projectEmployee")
                .GroupBy(p=>p.Element("project").Element("name").Value)
                .Select(p=> new 
                {
                    ProjectName = p.Key,
                    EmployeesNumber = p.Count()
                })
                .GroupBy(p => p.EmployeesNumber)
                .Select(u =>
                new QueriesPrinter
                {
                    FieldOne = u.Key.ToString(),
                    FieldTwo = u.Select(p => p.ProjectName).Aggregate((s1, s2) => s1 + " , " + s2)
                });

           
            return numberOfEmployees;
        }
        /// <summary>
        /// 6
        /// Grouping by Employees Position and finding their percentage among all Positions
        /// </summary>
        public IEnumerable<QueriesPrinter> PercentageOfPositions()
        {
            var allEmployyesCount = _employees.Elements("employee").Count();


            var percentagePos = from emp in _employees.Elements("employee")
                                group emp by emp.Element("position").Value into empPositions
                                select new QueriesPrinter
                                {
                                    FieldOne = empPositions.Key,
                                    FieldTwo = ((double)empPositions.Count() / allEmployyesCount * 100).ToString()
                                };
            return percentagePos;
        }

        /// <summary>
        /// 7
        /// Finding owners Income depending on the project cost
        /// </summary>
        public IEnumerable<QueriesPrinter> EmployerMoney()
        {

            var empMoney = from result in (from owners in _owners.Elements("owner")
                                           group owners by owners.Element("project").Element("name").Value into ownersProj
                                           select new
                                           {
                                               ProjectName = ownersProj.Key,
                                               FullName = from ownPj in ownersProj
                                                          select new string (ownPj.Element("name").Value + " " + ownPj.Element("surname").Value),
                                               Salary =Decimal.Parse(_projects.Elements("project").First(p => p.Element("name").Value == ownersProj.Key)
                                               .Element("projectCost").Value) /
                                                ownersProj.Count() / 40 * 100
                                           })
                           orderby result.Salary
                           select new QueriesPrinter
                           {
                               FieldOne = result.ProjectName,
                               FieldTwo = result.FullName.Count() < 2
                               ? result.FullName.First() + $" - Salary: {result.Salary}"
                               : result.FullName.Aggregate((s1, s2) => s1 + $" - Salary: {result.Salary} \n" + s2 + $" - Salary: {result.Salary}")
                           };
            return empMoney;

        }
        /// <summary>
        /// 8
        /// Rating Employees depending on their salary
        /// </summary>
        public IEnumerable<QueriesPrinter> EmployeeRating()
        {
            var employeeRating = _projectsEmployees.Elements("projectEmployee")
                                    .GroupBy(p => p.Element("employee").Element("surname").Value +" "+ p.Element("employee").Element("name").Value)
                                    .Select(p => new
                                    {
                                        FieldOne = p.Key,
                                        FieldTwo = p.Sum(u => Decimal.Parse(u.Element("project").Element("projectCost").Value) /
                                        _projectsEmployees.Elements("projectEmployee")
                                        .Count(e => e.Element("project").Element("code").Value == u.Element("project").Element("code").Value)) / 60 * 100
                                    })
                                    .OrderByDescending(p => p.FieldTwo)
                                    .Select(p => new QueriesPrinter
                                    {
                                        FieldOne = p.FieldOne,
                                        FieldTwo = (Decimal.Round(p.FieldTwo)).ToString()
                                    });

            return employeeRating;


        }
        /// <summary>
        /// 9
        /// The most popular month of Starting projects
        /// </summary>
        public string MostPopularStartingMonth()
        {
            var startMonth = _projects.Elements("project").Select(p => new
            {
                Name = p.Element("name").Value,
                Owners = p.Element("owners").Elements("owner").
                Select(o => o.Element("name").Value + " " + o.Element("surname").Value).Aggregate((s1, s2) => s1 + "," + s2),
                Month = DateTime.Parse(p.Element("startDate").Value).Month
            }).GroupBy(d => d.Month).
            Select(p => new
            {
                Month = p.Key,
                Count = p.Count(),
                Name = p.Select(n => n.Name).Aggregate((s1, s2) => s1 + "," + s2),
                Owners = p.Select(n => n.Owners).Aggregate((s1, s2) => s1 + "," + s2)
            }).OrderByDescending(p => p.Count).First();

            Console.WriteLine("\nQueue 9");
           return ($"\nMonth - {startMonth.Month}, " +
                $"\nAmount of projects - {startMonth.Count}, " +
                $"\nName - {startMonth.Name}, " +
                $"\nOwners - {startMonth.Owners}");

        }
 
        /// <summary>
        /// 10
        /// Finding employees who don`t work on First and Last projects
        /// </summary>
        public IEnumerable<QueriesPrinter> DontWorkEmployee()
        {
         
            var dontWorkEmpl = _employees.Elements("employee").Select(e => new QueriesPrinter
            {
                FieldOne = e.Element("name").Value,
                FieldTwo = e.Element("surname").Value
            }).
                Except(_projectsEmployees.Elements("projectEmployee")
                .Where(p=>p.Element("project").Element("code").Value ==
                    _projects.Elements("project").First().Element("code").Value)
                .Select(e => new QueriesPrinter
                {
                    FieldOne = e.Element("employee").Element("name").Value,
                    FieldTwo = e.Element("employee").Element("surname").Value
                }).
                Union(_projectsEmployees.Elements("projectEmployee")
                .Where(p => p.Element("project").Element("code").Value ==
                    _projects.Elements("project").Last().Element("code").Value)
                .Select(e => new QueriesPrinter
                {
                    FieldOne = e.Element("employee").Element("name").Value,
                    FieldTwo = e.Element("employee").Element("surname").Value
                })));
            return dontWorkEmpl;
        }
        /// <summary>
        /// 11
        /// Finding Avarage project cost per month
        /// </summary>
        public IOrderedEnumerable<QueriesPrinter> AverageProjectCost()
        {
            var averageProjCost = from result in (from projects in _projects.Elements("project")
                                                  select new QueriesPrinter
                                                  {
                                                      FieldOne = projects.Element("name").Value,
                                                      FieldTwo = Decimal.Round(Decimal.Parse(projects.Element("projectCost").Value) / (decimal)
                                                      (DateTime.Parse(projects.Element("endDate").Value) > DateTime.Now ?
                                                      (DateTime.Now.Subtract
                                                      (DateTime.Parse(projects.Element("startDate").Value)).Days / (365.25 / 12)) :
                                                      (DateTime.Parse(projects.Element("endDate").Value).Subtract(DateTime.Parse(projects.Element("startDate").Value)).Days 
                                                      / (365.25 / 12)))).ToString()
                                                  })
                                  orderby result.FieldTwo descending
                                  select result;

            return averageProjCost;

        }
        //fall
        /// <summary>
        /// 12
        /// Finding General salary of all workers on a certain Position
        /// </summary>
        public IEnumerable<QueriesPrinter> PositionCost()
        {
            var positionCost = _projectsEmployees.Elements("projectEmployee")
                                    .GroupBy(p => p.Element("employee").Element("position").Value)
                                    .Select(p => new QueriesPrinter
                                    {
                                        FieldOne = p.Key,
                                        FieldTwo = Decimal.Round(p.Sum(u => Decimal.Parse(u.Element("project").Element("projectCost").Value) /
                                        _projectsEmployees.Elements("projectEmployee")
                                        .Count(e => e.Element("project").Element("code").Value == u.Element("project").Element("code").Value)) / 60 * 100).ToString()
                                    });
                                   
            return positionCost;
        }
        /// <summary>
        /// 13
        /// Find and union all stakeholders to a certain project
        /// </summary>
        /// //fall
        public IEnumerable<QueriesPrinter> AllStakeholders()
        {
            var allStakeholders = _projects.Elements("project").Select(e => new QueriesPrinter
            {
                FieldOne = e.Element("name").Value,
                FieldTwo = e.Element("employees").Elements("employee").
                Select(e => new string (e.Element("name").Value + " " + e.Element("surname").Value))
                .Union(e.Element("owners").Elements("owner").
                Select(o => new string (o.Element("name").Value + " " + o.Element("surname").Value))).Aggregate((s1,s2)=> s1+ "\n" +s2)
            });
            return allStakeholders;

        }
        /// <summary>
        /// 14
        /// Finding the percantage each project has depending on a general cost of all projects
        /// </summary>
        public IEnumerable<QueriesPrinter> PercentageCostProjects()
        {
            var percentageCostProjects = from projects in _projects.Elements("project")
                                         select new QueriesPrinter
                                         {
                                             FieldOne = projects.Element("name").Value,
                                             FieldTwo = Decimal.Round(Decimal.Parse(projects.Element("projectCost").Value) * 100 / 
                                             (from prCost in _projects.Elements("project")
                                              select Decimal.Parse(prCost.Element("projectCost").Value)).Sum(), 2).ToString()
                                         };

            return percentageCostProjects;
        }
        //fall
        /// <summary>
        /// 15
        /// Finding best proposed project for an employee depending on his possible Salary
        /// </summary>
        public IEnumerable<QueriesPrinter> BestProposal()
        {
            var bestProp = _projectsEmployees.Elements("projectEmployee")
                .GroupBy(p => p.Element("employee").Element("name").Value + " " + p.Element("employee").Element("surname").Value)
                .Select(u => new QueriesPrinter
                {
                    FieldOne = u.Key,
                    FieldTwo = _projects.Elements("project").Select(o => o.Element("code").Value)
                    .Except(
                        u.Select(p => p.Element("project").Element("code").Value)
                        )
                    .Any()
                    ?
                    _projects.Elements("project").Select(o => o.Element("code").Value)
                    .Except(
                        u.Select(p => p.Element("project").Element("code").Value)
                        )
                    .Select(p => Decimal.Round(Decimal.Parse(_projectsEmployees.Elements("projectEmployee")
                    .First(o => o.Element("project").Element("code").Value == p).Element("project").Element("projectCost").Value) /
                    (_projectsEmployees.Elements("projectEmployee").Count(a => a.Element("project").Element("code").Value == p) + 1)/60*100))
                    .OrderBy(p=>p)
                    .Select(p=>p.ToString())
                    .Aggregate((s1,s2)=>s1+","+s2)
                    : "Already on all projects"


                });

            return bestProp;
        }

    }

    public class QueriesPrinter
    {
        public string FieldOne { get; init; }
        public string FieldTwo { get; init; }
    }

}
