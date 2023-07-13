using System;
namespace AttendanceTracker.Api.ViewModels
{
	public class CardViewModel
	{
		public string CardRefId { get; set; }

        public int EmployeeId { get; set; }

        public string ReasonNote { get; set; }


        public bool Status { get; set; }
	}
}

