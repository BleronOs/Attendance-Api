using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Entities.Account;

namespace AttendanceTracker.Core.Entities
{
    [Table("employee")]
    public class Employee : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [Column("first_name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        [Column("last_name")]
        public string LastName { get; set; }
        [Required]
        [Column("birthdate")]
        public DateTime BirthDate { get; set; }
        [Required]
        [Column("personal_number")]
        public long PersonalNumber { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("address")]
        public string Address { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; }
        [Required]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("position_id")]
        public int PositionId  { get; set; }
        [Required]
        [Column("status")]
        public bool Status { get; set; }
        [Column("userId")]
        public string UserId { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [ForeignKey("PositionId")]
        public JobPosition JobPosition { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<Card> Cards{ get; set; }
        public EmployeeManagment EmployeeManagment { get; set; }
        public Manager Manager { get; set; }
    }
}

