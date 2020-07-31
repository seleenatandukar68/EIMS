using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class TaskModel
    {
       

        public int prefId { get; set; }
        public int WeekId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDes { get; set; }
        public bool Status { get; set; }
    }
}