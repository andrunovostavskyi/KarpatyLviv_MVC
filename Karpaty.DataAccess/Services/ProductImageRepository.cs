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
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductImage productImage)
        {
            _db.Update(productImage);
        }
    }
}
