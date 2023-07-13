using System;
namespace AttendanceTracker.Api.Models
{
	public class ConfirmEmail
	{
		public string Token { get; set; }
		public string Email { get; set; }
	}
}

