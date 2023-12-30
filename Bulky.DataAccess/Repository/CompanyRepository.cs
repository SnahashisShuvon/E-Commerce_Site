using ECommerceSite.DataAccess.Data;
using ECommerceSite.DataAccess.Repository.IRepository;
using ECommerceSite.Models;

namespace ECommerceSite.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }
    }
}
