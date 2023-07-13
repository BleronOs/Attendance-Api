using System;
namespace AttendanceTracker.Api.ViewModels
{
	public class EmployeeViewModel
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime BirthDate { get; set; }

		public long PersonalNumber { get; set; }

		public string Address { get; set; }

		public string Email { get; set; }
		public string? NewEmail { get; set; }

		public string PhoneNumber { get; set; }

		public int PositionId { get; set; }

        public bool Status { get; set; }

        public string? Notes { get; set; }


    }
}

