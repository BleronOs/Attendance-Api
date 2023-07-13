
using System;
using AttendanceTracker.Core.Entities.Account;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceTracker.Core.Entities
{
    [Table("remarks")]
    public class Remarks: IEntity
	{
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Required]
        [Column("notes")]
        public string Notes { get; set; }
        [Column("inserted_datetime")]
        public DateTime InsertedDateTime { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}

