using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameZoneErp.Shared.Enums;

namespace GameZoneErp.Shared.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        
        public ProductType Type { get; set; } = ProductType.Standard;
        
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
