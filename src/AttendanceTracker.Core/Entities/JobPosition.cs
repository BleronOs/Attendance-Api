using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;

namespace AttendanceTracker.Core.Entities
{
    [Table("job_position")]
	public class JobPosition:IEntity
	{
		[Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("position_name")]
        [MaxLength(25)]
        public string PositionName { get; set; }
        [Required]
        [Column("status")]
        public bool Status { get; set; }

	}
}

