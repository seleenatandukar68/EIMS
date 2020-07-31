using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Models
{
    public class CommentModel
    {
        public int cmtId { get; set; }
        public int internId { get; set; }
        public int weekId { get; set; }
        public int projId { get; set; }
        public string addedOn { get; set; }
        [Required(ErrorMessage = "Please enter the CommentS")]
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
    }
}