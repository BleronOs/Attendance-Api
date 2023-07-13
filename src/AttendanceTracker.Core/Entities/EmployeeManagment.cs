using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;

namespace AttendanceTracker.Core.Entities
{
    [Table("employee_managment")]
	public class EmployeeManagment:IEntity
	{
		[Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("manager_id")]
        public int ManagerId { get; set; }
        [Required]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
		[ForeignKey("ManagerId")]
		public Manager Manager { get; set; }
		[ForeignKey("EmployeeId")]
		public Employee Employee { get; set; }

	}
}

