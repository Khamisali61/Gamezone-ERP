using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GameZoneErp.Shared.Entities
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public int? ParentId { get; set; }
        
        // Navigation property for self-referencing
        public Category? Parent { get; set; }
        public List<Category> Children { get; set; } = new();
    }
}
