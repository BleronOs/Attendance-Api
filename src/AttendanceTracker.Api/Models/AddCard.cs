using System;
namespace AttendanceTracker.Api.Models
{
	public class AddCard
	{
        public string CardRefId { get; set; }

        public int EmployeeId { get; set; }

        public string ReasonNote { get; set; }

        public bool Status { get; set; }
    }
}

