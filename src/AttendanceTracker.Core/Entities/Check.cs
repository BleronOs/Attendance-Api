using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Entities.Account;
namespace AttendanceTracker.Core.Entities
{
    [Table("check")]
	public class Check : IEntity
    {
		[Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("check_datetime")]
        public DateTime CheckDateTime { get; set; }
        [Column("server_datetime")]
        public DateTime ServerDateTime { get; set; }
        [Column("card_id")]
        public int CardId { get; set; }
        [Column("admin_id")]
        public string? AdminId { get; set; }
        [Column("note")]
        public string? Note { get; set; }
		[ForeignKey("CardId")]
		public Card Card { get; set; }
        [ForeignKey("AdminId")]
        public User User { get; set; }
	}
}

