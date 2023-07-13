using System;
namespace AttendanceTracker.Api.Models
{
	public class AddEmployee
	{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public long PersonalNumber { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int PositionId { get; set; }

        public bool Status { get; set; }

        public string Role { get; set; }


    }
}

