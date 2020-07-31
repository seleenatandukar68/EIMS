using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Models
{
    public class ProjectModel
    {
        public int ProjId { get; set; }
        [Required(ErrorMessage = "Please enter the Project Name")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please enter the status")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "Please enter the Start Date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please enter the End Date")]
        public DateTime EndDate { get; set; }
        public Boolean WasDeleted { get; set; }
    }
}