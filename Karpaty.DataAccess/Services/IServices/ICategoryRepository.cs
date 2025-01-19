using Karpaty.Models;

namespace Karpaty.Services.IServices
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
    }
}
