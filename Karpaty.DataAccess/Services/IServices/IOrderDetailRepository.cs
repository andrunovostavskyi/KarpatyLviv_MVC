using Karpaty.Models;
using Karpaty.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.DataAccess.Services.IServices
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail); 
    }
}
