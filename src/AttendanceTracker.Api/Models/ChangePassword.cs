using System;
namespace AttendanceTracker.Api.Models
{
	public class ChangePassword
	{
		public string UserId { get; set; }

		public string CurrentPassword { get; set; }

		public string NewPassword { get; set; }
	}
}

