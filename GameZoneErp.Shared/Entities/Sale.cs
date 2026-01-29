using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using GameZoneErp.Shared.Enums;

namespace GameZoneErp.Shared.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }
        
        public List<SaleItem> SaleItems { get; set; } = new();
    }
}
