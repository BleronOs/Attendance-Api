using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;

namespace AttendanceTracker.Core.Entities
{
	[Table("card")]
	public class Card : IEntity
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Required]
		[Column("card_ref_id")]
		public string CardRefId { get; set; }
        [Required]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
		[Column("reason_note")]
		public string? ReasonNote { get; set; }
		[Column("note")]
        public string? Note { get; set; }
		[Column("status")]
        public bool Status { get; set; }

		[ForeignKey("EmployeeId")]
		public Employee Employee { get; set; }

		public DateTime? InsertedDateTime { get; set; }
		public DateTime? UpdatedDateTime { get; set; }
	}
}

