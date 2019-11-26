using MobileBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileBackend.Controllers
{
    public class EmployeeController : ApiController
    {
        public string[] GetAll()
        {
            string[] employeeNames = null;
            TimesheetEntities entities = new TimesheetEntities();
            try
            {
                employeeNames = (from e in entities.Employees
                                 where (e.Active == true)
                                 select e.FirstName + " " +
                                 e.LastName).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return employeeNames;
        }

        public byte[] GetEmployeeImage(string employeeName)
        {
            TimesheetEntities entities = new TimesheetEntities();
            try
            {
                string[] nameParts = employeeName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string first = nameParts[0];
                string last = nameParts[1];
                byte[] bytes = (from e in entities.Employees
                                where (e.Active == true) &&
                                (e.FirstName == first) &&
                                (e.LastName == last)
                                select e.EmployeePicture).FirstOrDefault();

                return bytes;
            }
            finally
            {
                entities.Dispose();
            }
        }

        public string PutEmployeeImage()
        {
            TimesheetEntities entities = new TimesheetEntities();
            try
            {
                Employee newEmployee = new Employee()
                {
                    FirstName = "Heebo",
                    LastName = "X",
                    EmployeePicture = File.ReadAllBytes(@"C:\Temp\Heebo.png")
                };
                entities.Employees.Add(newEmployee);
                entities.SaveChanges();

                return "OK!";
            }
            finally
            {
                entities.Dispose();
            }

            return "Error";
        }
    }
}