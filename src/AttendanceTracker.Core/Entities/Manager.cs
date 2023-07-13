using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;

namespace AttendanceTracker.Core.Entities
{
	[Table("manager")]
	public class Manager:IEntity
	{
		[Key]
		[Column("id")]
        public int Id { get; set; }
		[Required]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
		[Required]
		[Column("status")]
		public bool Status { get; set; }
		[ForeignKey("EmployeeId")]
		public Employee Employee { get; set; }
    }
}

