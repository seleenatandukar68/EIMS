//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EIMS.DataModel
{
    using System;
    
    public partial class getUnreadComment_Result
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
        public string InternId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> WeekId { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public string Feedback { get; set; }
    }
}
