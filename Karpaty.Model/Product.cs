using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }
        public int Discount { get; set; }
        public const int discount50 = 10;
        public const int discount100 = 15;

        [Required] 
        public int Count { get; set; }
        
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category? Category { get; set; }
        [ValidateNever]
        public List<ProductImage> Images { get; set; }
    }
}
