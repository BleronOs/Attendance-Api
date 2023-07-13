using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceTracker.Api.Models
{
	public class AddRemarks
	{
        public int EmployeeId { get; set; }

        public string Notes { get; set; }
    }
}

