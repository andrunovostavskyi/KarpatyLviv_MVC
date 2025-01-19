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
	public class OrderHeader
	{
		public int Id { get; set; }

        public string AplicationUserId { get; set; }
        [ForeignKey("AplicationUserId")]
        [ValidateNever]
        public AplicationUser AplicationUser { get; set; }


		public DateTime OrderDate { get; set; }
		public DateTime ShippingDate { get; set; }
		public double OrderTotal { get; set; }

		public string? OrderStatus { get; set; }
		public string? TrackingNumber { get; set; }
		public string? PaymentStatus { get; set; }
		public string? Carried { get; set; }

		public DateTime PaymentDate { get; set; }
		public DateTime PaymentDueDate { get; set; }

		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }

		[Required]
        [MaxLength(100)]
        public string Name { get; set; }
		[Required]
		[MaxLength(100)]
		public string Email { get; set; }
		[Required]
		[MaxLength(100)]
		public string PhoneNumber { get; set; }
		[Required]
		[MaxLength(100)]
		public string Country { get; set; }
		[Required]
		[MaxLength(100)]
		public string City { get; set; }
		[Required]
		[MaxLength(100)]
		public string Adress { get; set; }
    }
}
