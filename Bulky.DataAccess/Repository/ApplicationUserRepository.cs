using ECommerceSite.DataAccess.Data;
using ECommerceSite.DataAccess.Repository.IRepository;
using ECommerceSite.Models;

namespace ECommerceSite.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _context.ApplicationUsers.Update(applicationUser);
        }
    }
}
