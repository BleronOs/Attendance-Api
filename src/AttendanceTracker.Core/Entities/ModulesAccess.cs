using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
namespace AttendanceTracker.Core.Entities
{
    [Table("modules_access")]
    public class ModulesAccess : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("module_id")]
        public int ModuleId { get; set; }
        [Required]
        [Column("role_id")]
        public string RoleId { get; set; }
        [Required]
        [Column("has_access")]
        public bool HasAccess { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [ForeignKey("ModuleId")]
        public Modules Modules { get; set; }

    }
}

 