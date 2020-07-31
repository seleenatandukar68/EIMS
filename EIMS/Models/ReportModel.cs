using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class ReportModel
    {
        public int? ProjectId { get; set; }
        public int MentorId { get; set; }
        public int? PrefId { get; set; }
        public int? WeekId { get; set; }
        public int? InternId { get; set;}
    }
}