using Readzy.DataAccess.Repository.IRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.OrderHeaderRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void Save();
        OrderHeader GetByIdWithInclude(int id);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
        List<OrderHeader> GetByAllWithInclude();
    }
}
