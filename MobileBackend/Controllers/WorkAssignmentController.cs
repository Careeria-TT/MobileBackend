using MobileBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimesheetMobile.Models;

namespace MobileBackend.Controllers
{
    public class WorkAssignmentController : ApiController
    {
        public string[] GetAll()
        {
            string[] assignmentNames = null;
            CareeriaMobileDBEntities entities = new CareeriaMobileDBEntities();
            try
            {
                assignmentNames = (from wa in entities.WorkAssignments
                                   where (wa.Active == true)
                                   select wa.Title).ToArray();
            }
            finally
            {
                entities.Dispose();
            }
            return assignmentNames;
        }

        [HttpPost]
        public bool PostStatus(WorkAssignmentOperationModel input)
        {

            CareeriaMobileDBEntities entities = new CareeriaMobileDBEntities();
            try
            {
                WorkAssignment assignment = (from wa in entities.WorkAssignments
                                   where (wa.Active == true) &&
                                   (wa.Title == input.AssignmentTitle)
                                   select wa).FirstOrDefault();

                if (assignment == null)
                {
                    return false;
                }

                if (input.Operation == "Start") { 
                int assignmentId = assignment.Id_WorkAssignment;
                Timesheet newEntry = new Timesheet()
                {
                    Id_WorkAssignment = assignmentId,
                    StartTime = DateTime.Now,
                    WorkComplete = false,
                    Active = true,
                    CreatedAt = DateTime.Now
                };
                entities.Timesheets.Add(newEntry);
                }
                else if (input.Operation == "Stop")
                {
                    int assignmentId = assignment.Id_WorkAssignment;
                    Timesheet existing = (from ts in entities.Timesheets
                                          where (ts.Id_WorkAssignment == assignmentId) &&
                                          (ts.Active == true)
                                          select ts).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.StopTime = DateTime.Now;
                        existing.WorkComplete = true;
                        existing.LastModifiedAt = DateTime.Now;
                    }
                    else
                    {
                        return false;
                    }
                }
                entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {
                entities.Dispose();
            }
            return true;
        }
    }
}
