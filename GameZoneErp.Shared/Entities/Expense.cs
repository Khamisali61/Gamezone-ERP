using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GameZoneErp.Shared.Entities
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Category { get; set; } = string.Empty; // e.g. Rent, Salary

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
