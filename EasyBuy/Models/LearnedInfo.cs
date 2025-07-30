using System.ComponentModel.DataAnnotations;

namespace EasyBuy.Models
{
    public class LearnedInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Keyword { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string Information { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
} 