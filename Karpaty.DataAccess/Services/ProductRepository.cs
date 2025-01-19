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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public void Update(Product product)
        {
            _db.Update(product);
        }
    }
}
