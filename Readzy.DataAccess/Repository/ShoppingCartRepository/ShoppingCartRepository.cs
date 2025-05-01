using Microsoft.EntityFrameworkCore;
using Readzy.DataAccess.Data;
using Readzy.DataAccess.Repository.ProductRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.ShoppingCartRepository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ReadzyContext context;
        public ShoppingCartRepository(ReadzyContext context) : base(context)
        {
            this.context = context;
        }

        public ShoppingCart GetById(int? id)
        {
            return context.ShoppingCarts.FirstOrDefault(sc => sc.Id == id);
        }

        public ShoppingCart GetFirstOrDefault(string userId, int productId)
        {
            return context.ShoppingCarts.FirstOrDefault(context => context.ApplicationUserId == userId && context.ProductId == productId);
        }

        public List<ShoppingCart> GetAllByUserIdWithIncludeProduct(string userId)
        {
            return context.ShoppingCarts
                .Include(sc => sc.Product)
                .Where(sc => sc.ApplicationUserId == userId).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ShoppingCart shoppingCart)
        {
            context.ShoppingCarts.Update(shoppingCart);
        }
    }
}
