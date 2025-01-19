using Karpaty.DataAccess.Services.IServices;

namespace Karpaty.Services.IServices
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category{ get; }
        IProductRepository Product { get; }
        IShoppingCardRepository ShoppingCard { get; }
        IAplicationUserRepository AplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IProductImageRepository ProductImage { get; }

        void Save();
    }
}
