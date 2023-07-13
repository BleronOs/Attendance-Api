using System;
namespace AttendanceTracker.Api.Models
{
    public class AddCheck
    {

        public DateTime CheckDateTime { get; set; }
        public int EmployeeId { get; set; }
        public string? AdminId { get; set; }
        public string? Note { get; set; }
    }
}

