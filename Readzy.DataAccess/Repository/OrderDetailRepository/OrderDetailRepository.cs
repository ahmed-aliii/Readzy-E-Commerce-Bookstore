using Microsoft.EntityFrameworkCore;
using Readzy.DataAccess.Data;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.OrderDetailRepository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ReadzyContext context;
        public OrderDetailRepository(ReadzyContext context) : base(context)
        {
            this.context = context;
        }
        public OrderDetail GetByIdWithIncludes(int id)
        {
            return context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.OrderHeader)
                .FirstOrDefault(od => od.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public void Update(OrderDetail orderDetail)
        {
            context.OrderDetails.Update(orderDetail);
        }
    }
}
