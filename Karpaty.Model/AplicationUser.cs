using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.Models
{
    public class AplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? StreetAdress { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
