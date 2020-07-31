using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Models
{
    public class ProjTaskModel
    {
        public int ProjId { get; set; }
        public string ProjectName { get; set; }

        public int WeekId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDes{get;set;}
        public int Status { get; set; }
        public int InternId { get; set; }

        [Required]
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}