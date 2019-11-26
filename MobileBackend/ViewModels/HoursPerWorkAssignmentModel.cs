using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileBackend.ViewModels
{
    public class HoursPerWorkAssignmentModel
    {
        public int WorkAssignmentId { get; set; }
        public string WorkAssignmentName { get; set; }
        public double TotalHours { get; set; }
    }
}