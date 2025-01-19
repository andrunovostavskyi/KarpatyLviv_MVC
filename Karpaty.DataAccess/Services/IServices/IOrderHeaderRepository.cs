using Karpaty.Models;
using Karpaty.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.DataAccess.Services.IServices
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus);
        void UpdateSessionPaymentId(int id, string sessioId, string paymentIntentId);
    }
}
