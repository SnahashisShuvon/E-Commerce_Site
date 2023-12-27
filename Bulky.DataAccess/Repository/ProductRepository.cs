using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Product category)
        {
            var objFromDb = _context.Products.FirstOrDefault(u => u.Id == category.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = category.Title;
                objFromDb.ISBN = category.ISBN;
                objFromDb.Price = category.Price;
                objFromDb.Price50 = category.Price50;
                objFromDb.ListPrice = category.ListPrice;
                objFromDb.Price100 = category.Price100;
                objFromDb.Description = category    .Description;
                objFromDb.CategoryId = category.CategoryId;
                objFromDb.Author = category.Author;
                objFromDb.ProductImages = category.ProductImages;
            }
        }
    }
}
