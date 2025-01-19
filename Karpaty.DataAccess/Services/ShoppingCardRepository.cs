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
    public class ShoppingCardRepository : Repository<ShoppingCard>, IShoppingCardRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCardRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public void Update(ShoppingCard shoppingCard)
        {
            _context.Update(shoppingCard);
        }
    }
}
