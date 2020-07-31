using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class ProjectTaskModel
    {
        public int prefId { get; set;}
        [Range(1, 12, ErrorMessage = "Enter number between 1 to 12")]
        public int weekId { get; set; }
        public ProjectModel project { get; set; }
        public List<ProjectModel> projectList{ get; set; }
        public List<TaskModel> tasks { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}