using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.LINQ.GeneralData
{
    public static class DataSeeding
    {
        public static Data Create()
        {
            var employees = new List<Employee>
            {
                //1
                new Employee
                {
                    Name = "Pavlo",
                    Surname = "Pavlichenko",
                    Position = Position.Designer,
                    Projects = new List<Project>()
                },
                //2
                new Employee
                {
                     Name = "Yana",
                    Surname = "Manhusheva",
                    Position = Position.DevOps,
                    Projects = new List<Project>()
                },
                //3
                new Employee
                {
                    Name = "Artem",
                    Surname = "Stankov",
                    Position = Position.Designer,
                    Projects = new List<Project>()
                },
                //4
                new Employee
                {
                    Name = "Olga",
                    Surname = "Pavlichenko",
                    Position = Position.PM,
                    Projects = new List<Project>()
                },
                //5
                new Employee
                {
                    Name = "Oryna",
                    Surname = "Otkalenko",
                    Position = Position.Developer,
                    Projects = new List<Project>()
                },
                //6
                new Employee
                {
                    Name = "Semen",
                    Surname = "Semenyuk",
                    Position = Position.Developer,
                    Projects = new List<Project>()
                },
                //7
                new Employee
                {
                    Name = "Ira",
                    Surname = "Sochavska",
                    Position = Position.DevOps,
                    Projects = new List<Project>()
                },
                //8
                new Employee
                {
                    Name = "Olia",
                    Surname = "Hoi",
                    Position = Position.PM,
                    Projects = new List<Project>()
                },
                //9
                new Employee
                {
                    Name = "Denis",
                    Surname = "Lienko",
                    Position = Position.Designer,
                    Projects = new List<Project>()
                },
                //10
                new Employee
                {
                    Name = "Vitalik",
                    Surname = "Vunnyk",
                    Position = Position.DevOps,
                    Projects = new List<Project>()
                }
            };


            var projects = new List<Project>
            {
                //1
                new Project
                {
                    Name = "New KPI",
                    Code = "FN4-T8",
                    StartDate = new DateTime(1994,7,26),
                    EndDate = new DateTime(2003,1,14),
                    Owners = new List<Owner>(),
                    Employees = new List<Employee>{ employees[7], employees[1], employees[0], employees[5] },
                    ProjectCost = 100000m
                },
                //2
                new Project
                {
                    Name = "Save the planet",
                    Code = "PP03",
                    StartDate = new DateTime(2012,4,15),
                    EndDate = new DateTime(2021,12,23),
                    Owners = new List<Owner>(),
                    Employees = new List<Employee>
                    {
                        employees[2], employees[3], employees[6], employees[9], employees[5]
                    },
                    ProjectCost = 340000m
                },
                //3
                new Project
                {
                    Name = "Aerospace Engineering and Digital Innovation Centre’ in India",
                    Code = "2345-A3-2345-11KBЗ",
                    StartDate = new DateTime(1999,3,10),
                    EndDate = new DateTime(2025,1,20),
                    Owners = new List<Owner>(),
                    Employees = new List<Employee>
                    {
                        employees[7], employees[1], employees[2], employees[5] , employees[4] , employees[8]
                    },
                    ProjectCost = 550000m

                },
                //4
                new Project
                {
                    Name = "Living Lab Ecosystem in Melbourne",
                    Code = "WPI.2.4-4:2022",
                    StartDate = new DateTime(2022,3,20),
                    EndDate = new DateTime(2030,8,19),
                    Owners = new List<Owner>(),
                    Employees = new List<Employee>{ employees[8], employees[0], employees[5], employees[1]},
                    ProjectCost = 1800000m
                }

            };

            var owners = new List<Owner>
            {
                //1
                new Owner
                {
                    Name = "Valentyn",
                    Surname ="Strukalo",
                    Project = projects[0]
                },
                //2
                new Owner
                {
                    Name = "Mikhail",
                    Surname ="Zgurovsky",
                    Project = projects[0]
                },
                //3
                new Owner
                {
                    Name = "Lana",
                    Surname ="del Ray",
                    Project = projects[3]
                },
                //4
                new Owner
                {
                    Name = "Oksana",
                    Surname ="Zachemenko",
                    Project = projects[1]
                },
                //5
                new Owner
                {
                    Name = "Lena",
                    Surname ="Nedilko",
                    Project = projects[2]
                },
                //6
                new Owner
                {
                    Name = "Oliver",
                    Surname ="Schwarz",
                    Project = projects[2]
                }
            };



            projects[0].Owners.AddRange(new List<Owner> { owners[0], owners[1] });
            projects[1].Owners.Add(owners[3]);
            projects[2].Owners.AddRange(new List<Owner> { owners[4], owners[5] });
            projects[3].Owners.Add(owners[2]);


            employees[0].Projects.AddRange(new List<Project> 
            {
                projects[0], projects[3] 
            });
            employees[1].Projects.AddRange(new List<Project> 
            { 
                projects[0], projects[2], projects[3] 
            });
            employees[2].Projects.AddRange(new List<Project> 
            {
                projects[1], projects[2]
            });
            employees[3].Projects.Add(projects[1]);
            employees[4].Projects.Add(projects[2]);
            employees[5].Projects.AddRange(new List<Project> 
            {
                projects[0], projects[1], projects[2], projects[3] 
            });
            employees[6].Projects.Add(projects[1]);
            employees[7].Projects.AddRange(new List<Project> 
            { 
                projects[0], projects[2]
            });
            employees[8].Projects.AddRange(new List<Project> 
            {
                projects[2], projects[3]
            });
            employees[9].Projects.Add(projects[1]);




            var projectsEmployees = new List<ProjectEmployee>
            {
                new ProjectEmployee
                {
                    Employee = employees[0],
                    Project = projects[0]
                },
                new ProjectEmployee
                {
                    Employee = employees[0],
                    Project = projects[3]
                },
                new ProjectEmployee
                {
                    Employee = employees[1],
                    Project = projects[0]
                },
                new ProjectEmployee
                {
                    Employee = employees[1],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[1],
                    Project = projects[3]
                },
                new ProjectEmployee
                {
                    Employee = employees[2],
                    Project = projects[1]
                },
                new ProjectEmployee
                {
                    Employee = employees[2],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[3],
                    Project = projects[1]
                },
                new ProjectEmployee
                {
                    Employee = employees[4],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[5],
                    Project = projects[0]
                },
                new ProjectEmployee
                {
                    Employee = employees[5],
                    Project = projects[1]
                },
                new ProjectEmployee
                {
                    Employee = employees[5],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[5],
                    Project = projects[3]
                },
                new ProjectEmployee
                {
                    Employee = employees[6],
                    Project = projects[1]
                },
                new ProjectEmployee
                {
                    Employee = employees[7],
                    Project = projects[0]
                },
                new ProjectEmployee
                {
                    Employee = employees[7],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[8],
                    Project = projects[2]
                },
                new ProjectEmployee
                {
                    Employee = employees[8],
                    Project = projects[3]
                },
                new ProjectEmployee
                {
                    Employee = employees[9],
                    Project = projects[1]
                },

            };

            return new Data { Employees = employees, Owners = owners, Projects = projects, ProjectsEmployees = projectsEmployees };
        }
    }
}


