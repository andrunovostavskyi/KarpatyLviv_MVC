using Karpaty.Data;
using Karpaty.DataAccess.Services.IServices;
using Karpaty.Models;
using Karpaty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.DataAccess.Services
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }

        public void UpdateSessionPaymentId(int id, string sessioId, string paymentIntentId)
        {
            var itemUpdate = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (!string.IsNullOrEmpty(sessioId))
            {
                itemUpdate.SessionId = sessioId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                itemUpdate.PaymentIntentId = paymentIntentId;
                itemUpdate.PaymentDate = DateTime.UtcNow;
            }
            _db.SaveChanges();
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var itemUpdate = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (itemUpdate != null)
            {
                itemUpdate.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(orderStatus))
                {
                    itemUpdate.PaymentStatus = paymentStatus;
                }
            }

            _db.SaveChanges();
        }
    }
}
