﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.Models
{
    public class ShoppingCard
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        public string? AplicationUserId { get; set; }
        [ForeignKey("AplicationUserId")]
        [ValidateNever] 
        public AplicationUser? AplicationUser { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

    }
}
