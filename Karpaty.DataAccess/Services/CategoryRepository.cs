using Karpaty.Data;
using Karpaty.Models;
using Karpaty.Services.IServices;

namespace Karpaty.Services
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(Category category)
        {
            dbset.Update(category);
        }
    }
}
