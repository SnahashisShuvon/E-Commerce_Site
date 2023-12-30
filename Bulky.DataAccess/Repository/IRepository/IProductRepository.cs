using ECommerceSite.Models;

namespace ECommerceSite.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product category);
    }
}
