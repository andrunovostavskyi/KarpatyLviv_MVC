using Karpaty.Data;
using Karpaty.DataAccess.Services;
using Karpaty.DataAccess.Services.IServices;
using Karpaty.Models;
using Karpaty.Services.IServices;

namespace Karpaty.Services
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCardRepository ShoppingCard {  get; private set; }
        public IAplicationUserRepository AplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ProductImage = new ProductImageRepository(db);
            AplicationUser = new AplicationUserRepository(db);
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
            OrderHeader = new OrderHeaderRepository(db);
            OrderDetail = new OrderDetailRepository(db);
            ShoppingCard = new ShoppingCardRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
