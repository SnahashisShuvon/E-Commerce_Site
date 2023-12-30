using ECommerceSite.DataAccess.Data;
using ECommerceSite.DataAccess.Repository.IRepository;
using ECommerceSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSite.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(ProductImage obj)
        {
            _context.ProductImages.Update(obj);
        }
    }
}
