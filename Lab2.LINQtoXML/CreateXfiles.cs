using Lab1.LINQ.GeneralData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab2.LINQtoXML
{
    static class CreateXfiles
    {
        static readonly XmlWriterSettings settings = new()
        {
            Indent = true
        };
        static public void CreateXemployees(Data data)
        {
            using (XmlWriter writer = XmlWriter.Create("employees.xml", settings))
            {
                writer.WriteStartElement("employees");
                foreach (var employee in data.Employees)
                {
                    writer.WriteStartElement("employee");
                    writer.WriteElementString("position", employee.Position.ToString());
                    writer.WriteElementString("name", employee.Name);
                    writer.WriteElementString("surname", employee.Surname);

                    
                    writer.WriteEndElement();


                }
                writer.WriteEndElement();
            }

        }
        static public void CreateXowners(Data data)
        {
            using (XmlWriter writer = XmlWriter.Create("owners.xml", settings))
            {
                writer.WriteStartElement("owners");
                foreach (var owner in data.Owners)
                {
                    writer.WriteStartElement("owner");
                    writer.WriteElementString("name", owner.Name);
                    writer.WriteElementString("surname", owner.Surname);

                    writer.WriteStartElement("project");
                    writer.WriteElementString("code", owner.Project.Code);
                    writer.WriteElementString("name", owner.Project.Name);
                    writer.WriteElementString("projectCost", owner.Project.ProjectCost.ToString());
                    writer.WriteElementString("startDate", owner.Project.StartDate.ToString());
                    writer.WriteElementString("endDate", owner.Project.EndDate.ToString());
                    writer.WriteStartElement("owners");
                    foreach (var projOwn in owner.Project.Owners)
                    {
                        writer.WriteStartElement("owner");
                        writer.WriteElementString("name", projOwn.Name);
                        writer.WriteElementString("surname", projOwn.Surname);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

        }
        static public void CreateXprojects(Data data)
        {
            using (XmlWriter writer = XmlWriter.Create("projects.xml", settings))
            {
                writer.WriteStartElement("projects");
                foreach (var project in data.Projects)
                {
                    writer.WriteStartElement("project");
                    writer.WriteElementString("code", project.Code);
                    writer.WriteElementString("name", project.Name);
                    writer.WriteElementString("projectCost", project.ProjectCost.ToString());
                    writer.WriteElementString("startDate", project.StartDate.ToString());
                    writer.WriteElementString("endDate", project.EndDate.ToString());

                  
                    writer.WriteStartElement("owners");
                    foreach (var owner in project.Owners)
                    {
                        writer.WriteStartElement("owner");
                        writer.WriteElementString("name", owner.Name);
                        writer.WriteElementString("surname", owner.Surname);

                        writer.WriteStartElement("project");
                        writer.WriteElementString("code", owner.Project.Code);
                        writer.WriteElementString("name", owner.Project.Name);
                        writer.WriteElementString("projectCost", owner.Project.ProjectCost.ToString());
                        writer.WriteElementString("startDate", owner.Project.StartDate.ToString());
                        writer.WriteElementString("endDate", owner.Project.EndDate.ToString());
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        static public void CreateXProjectsEmployees(Data data)
        {
            using (XmlWriter writer = XmlWriter.Create("projectsEmployees.xml", settings))
            {
                writer.WriteStartElement("projectsEmployees");
                foreach (var projectEmployee in data.ProjectsEmployees)
                {
                    writer.WriteStartElement("projectEmployee");

                    writer.WriteStartElement("project");
                    writer.WriteElementString("code", projectEmployee.Project.Code);
                    writer.WriteElementString("name", projectEmployee.Project.Name);
                    writer.WriteElementString("projectCost", projectEmployee.Project.ProjectCost.ToString());
                    writer.WriteElementString("startDate", projectEmployee.Project.StartDate.ToString());
                    writer.WriteElementString("endDate", projectEmployee.Project.EndDate.ToString());
                    writer.WriteStartElement("owners");
                    foreach (var owner in projectEmployee.Project.Owners)
                    {
                        writer.WriteStartElement("owner");
                        writer.WriteElementString("name", owner.Name);
                        writer.WriteElementString("surname", owner.Surname);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();


                    writer.WriteStartElement("employee");
                    writer.WriteElementString("position", projectEmployee.Employee.Position.ToString());
                    writer.WriteElementString("name", projectEmployee.Employee.Name);
                    writer.WriteElementString("surname", projectEmployee.Employee.Surname);
                    writer.WriteEndElement();


                    writer.WriteEndElement();

                }


                writer.WriteEndElement();
            }
        }
        static public void CreateXelements(Data data)
        {
            CreateXemployees(data);
            CreateXowners(data);
            CreateXprojects(data);
            CreateXProjectsEmployees(data);
        }
    }
}
