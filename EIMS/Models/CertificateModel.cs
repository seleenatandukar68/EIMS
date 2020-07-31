using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class CertificateModel
    {
        public string FName { get; set; }
        public string MName { get; set; }
      
        public string LName { get; set; }
        public string InternId { get; set; }
        public DateTime internStartDate { get; set; }
        public DateTime internEndDate { get; set; }
        public string Remarks { get; set; }
        public string prefCategory { get; set; }
    }
}