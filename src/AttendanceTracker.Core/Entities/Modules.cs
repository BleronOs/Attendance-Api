using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceTracker.Core.Interfaces;

namespace AttendanceTracker.Core.Entities
{
    [Table("modules")]
    public class Modules: IEntity
    {
        [Key]
		[Column("id")]
        public int Id { get; set; }
        [Column("module_name")]
        public string ModuleName { get; set; }
    }
}

