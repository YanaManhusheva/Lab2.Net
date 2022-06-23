using Lab1.LINQ.GeneralData;
using System;
using System.Xml;
using System.Linq;
using Models;

namespace Lab2.LINQtoXML
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = DataSeeding.Create();
            CreateXfiles.CreateXelements(data);
            var queues = new Queues();
            ChooseOption(queues, data);
        }


        static public void AddNewOwner(Data data)
        {
            bool choosen = false;
            while (!choosen)
            {
                Console.WriteLine("Enter owner`s name");
                var ownerName = Console.ReadLine();
                if (String.IsNullOrEmpty(ownerName) || ownerName.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mistake. Incorrect input");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }
                Console.WriteLine("Enter owner`s surname");
                var ownerSurname = Console.ReadLine();
                if (String.IsNullOrEmpty(ownerSurname) || ownerSurname.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mistake. Incorrect input");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }

                Console.WriteLine("Choose your project:");
                var num = 1;
                foreach (var projectName in data.Projects)
                {
                    Console.WriteLine($"{num}. '{projectName.Name}'");
                    num++;
                }
                var InputprojectId = Console.ReadLine();
                if (InputprojectId is null || !int.TryParse(InputprojectId, out int projectId) 
                    || projectId > data.Projects.Count() || projectId < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mistake. Incorrect input");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }


                var project = data.Projects.ElementAt(projectId - 1);
                var owner = new Owner
                {
                    Name = ownerName,
                    Surname = ownerSurname,
                    Project = project
                };
                data.Owners = data.Owners.Append(owner);
                project.Owners.Add(owner);

                CreateXfiles.CreateXowners(data);
                CreateXfiles.CreateXprojects(data);
                CreateXfiles.CreateXProjectsEmployees(data);

                choosen = true;
            }

        }
        static public void ChooseOption(Queues queues, Data data)
        {
            var chosen = false;
            while (!chosen)
            {
                Console.WriteLine("Choose option 1-16 \t0 to Exit");
                string option = Console.ReadLine();
                if (option is null || !int.TryParse(option, out int res) || res > 16 || res < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mistake. Incorrect input");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }
                switch (res)
                {
                    case 1:
                        {
                            Console.WriteLine("See joined owners of the project with their project and then grouping by projects Name.");
                            var query = queues.JoinOwnerProject();
                            Console.WriteLine("\nQueue 1");
                            foreach (var project in query)
                            {
                                Console.WriteLine($"\nProjectName - {project.FieldOne} \nOwners:");
                                Console.WriteLine($"{project.FieldTwo}");
                            }
                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine("See Employyes who work on both projects.");
                            var query = queues.IntersectProjectsEmployees();
                            Console.WriteLine("\nQueue 2");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"\nEmployee {item.FieldOne} {item.FieldTwo} on both " +
                                   $"{data.Projects.ElementAt(0).Name} and {data.Projects.ElementAt(1).Name} projects ");
                            }
                        }
                    break;
                    case 3:
                        {
                            Console.WriteLine("See the duration of finished projects and sorting them by EndDate.");
                            var query = queues.FilterDateProjects();
                            Console.WriteLine("\nQueue 3");
                            Console.WriteLine();
                            foreach (var a in query)
                            {
                                Console.WriteLine($"ProjectName: {a.FieldOne} - Duration: {a.FieldTwo} days");
                            }
                        }
                        break;
                    case 4:
                        {
                            Console.WriteLine("See the most popular position among Projects.");
                            var query =  queues.MostPopularPosition();
                            Console.WriteLine("\nQueue 4");
                            Console.WriteLine($"\nPosition : {query.FieldOne} \nEmployees Number : {query.FieldTwo}");

                        }
                        break;
                    case 5:
                        {
                            Console.WriteLine("See the amount of workers on each project grouped by the number of employees.");
                            var query = queues.NumberEmployees();
                            Console.WriteLine("\nQueue 5");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"\nEmployees Number : {item.FieldOne} \nProjectsNames : {item.FieldTwo}");
                            }
                        }
                        break;
                    case 6:
                        {
                            Console.WriteLine("See the percentage of employees on certain position in compare to all Positions.");
                            var query = queues.PercentageOfPositions();
                            Console.WriteLine("\nQueue 6");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"Position : {item.FieldOne} - {item.FieldTwo}%");
                            }
                        }
                        break;
                    case 7:
                        {
                            Console.WriteLine("See owners Income depending on the project cost.");
                            var query = queues.EmployerMoney();
                            Console.WriteLine("\nQueue 7");
                            foreach (var proj in query)
                            {
                                Console.WriteLine($"\nProject Name : {proj.FieldOne} ");
                                Console.WriteLine($" {proj.FieldTwo} ");
                            }
                        }
                        break;
                    case 8:
                        {
                            Console.WriteLine("See the rate of Employees depending on their salary.");
                            var query = queues.EmployeeRating();
                            Console.WriteLine("\nQueue 8");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"Employee : {item.FieldOne} \tSalary : {item.FieldTwo}");
                            }
                        }
                        break;
                    case 9:
                        Console.WriteLine("See most popular month of Starting projects.");
                        Console.WriteLine(queues.MostPopularStartingMonth());
                        break;
                    case 10:
                        {
                            Console.WriteLine("See employees who don`t work on First and Last projects.");
                            var query = queues.DontWorkEmployee();

                            Console.WriteLine("\nQueue 10");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"Employee : {item.FieldOne} {item.FieldTwo} ");
                            }
                            Console.WriteLine("\tdon`t work on" +
                                    $"'{data.Projects.First().Name}' and " +
                                    $"'{data.Projects.Last().Name}'");
                        }
                        
                        break;
                    case 11:
                        {
                            Console.WriteLine("See Avarage project cost per month.");
                            var query = queues.AverageProjectCost();
                            Console.WriteLine("\nQueue 11");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"\nName : {item.FieldOne}  \nAvegareCost : {item.FieldTwo}$ per month");
                            }
                        }
                        break;
                    case 12:
                        {
                            Console.WriteLine("See General salary of all workers on a certain Position.");
                            var query = queues.PositionCost();
                            Console.WriteLine("\nQueue 12");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"Position : {item.FieldOne} \tGeneralSalary : {item.FieldTwo}$");
                            }
                        }
                        break;
                    case 13:
                        {
                            Console.WriteLine("See all stakeholders to a certain project.");
                            var query = queues.AllStakeholders();
                            Console.WriteLine("\nQueue 13");
                            foreach (var proj in query)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Project : {proj.FieldOne} \nStakeholders : ");
                                Console.WriteLine(proj.FieldTwo);
                            }
                        }
                        break;
                    case 14:
                        {
                            Console.WriteLine("See the percantage each project has depending on a general cost of all projects.");
                            var query = queues.PercentageCostProjects();
                            Console.WriteLine("\nQueue 14");
                            foreach (var item in query)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Project : {item.FieldOne}  \nPercentage : {item.FieldTwo}%");
                            }
                        }
                        break;
                    case 15:
                        {
                            Console.WriteLine("See best proposed project for an employee depending on his possible Salary.");
                            var query = queues.BestProposal();
                            Console.WriteLine("\nQueue 15");
                            foreach (var emp in query)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Employee : {emp.FieldOne}");
                                Console.WriteLine($"Salary: {emp.FieldTwo}");
                            }
                        }
                        break;
                    case 0:
                        Console.WriteLine("Finished");
                        chosen = true;
                        break;
                    case 16:
                        AddNewOwner(data);
                        queues.UpdateXOwners();
                        queues.UpdateXProjects();
                        queues.UpdateXProjectsEmployees();
                        break;
                }
            }
        }
    }
}
