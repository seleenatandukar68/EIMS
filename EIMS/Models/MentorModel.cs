using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class MentorModel
    {
        public int Id { get; set; }
        public String MentorId { get; set; }
        [Required(ErrorMessage = "Please enter the First Name")]
        public string FName { get; set; }
        public string MName { get; set; }
        [Required(ErrorMessage = "Please enter the Last Name")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Please enter the Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email ID is Required")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]

        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
        public string Contact { get; set; }
        [Required(ErrorMessage = "Enter the id of Assigned Project ")]
        public int ProjId { get; set; }

    }
}