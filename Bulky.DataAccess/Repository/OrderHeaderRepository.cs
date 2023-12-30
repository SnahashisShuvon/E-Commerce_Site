using ECommerceSite.DataAccess.Data;
using ECommerceSite.DataAccess.Repository.IRepository;
using ECommerceSite.Models;

namespace ECommerceSite.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderHeaderFromDb = _context.OrderHeaders.Where(orderHeader => orderHeader.Id == id).FirstOrDefault();
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderHeaderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderHeaderFromDb = _context.OrderHeaders.Where(orderHeader => orderHeader.Id == id).FirstOrDefault();
            if (orderHeaderFromDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    orderHeaderFromDb.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderHeaderFromDb.PaymentIntentId = paymentIntentId;
                    orderHeaderFromDb.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
