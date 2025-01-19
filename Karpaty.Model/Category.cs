using System.ComponentModel.DataAnnotations;

namespace Karpaty.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(125)]
        public string? Name { get; set; }

        public int DisplayNumber { get; set; }
    }
}
