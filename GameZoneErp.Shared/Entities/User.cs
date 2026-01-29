using GameZoneErp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameZoneErp.Shared.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public UserRole Role { get; set; }
        
        public string PasswordHash { get; set; } = string.Empty;
    }
}
