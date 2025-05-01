using Microsoft.EntityFrameworkCore;
using Readzy.DataAccess.Data;
using Readzy.DataAccess.Repository.ProductRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.OrderHeaderRepository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ReadzyContext context;
        public OrderHeaderRepository(ReadzyContext context) : base(context)
        {
            this.context = context;
        }

        public List<OrderHeader> GetByAllWithInclude()
        {
            return context.OrderHeaders
                .Include(o => o.ApplicationUser)
                .ToList();
        }

        public OrderHeader GetByIdWithInclude(int id)
        {
            return context.OrderHeaders.Include(o => o.ApplicationUser).FirstOrDefault(o => o.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(OrderHeader orderHeader)
        {
            context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderHeaderFromDb = context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderHeaderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderHeaderFromDb = context.OrderHeaders.FirstOrDefault(o => o.Id == id);
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
