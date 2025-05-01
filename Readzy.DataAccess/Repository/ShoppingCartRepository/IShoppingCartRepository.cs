using Readzy.DataAccess.Repository.IRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.ShoppingCartRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
        void Save();
        ShoppingCart GetById(int? id);
        ShoppingCart GetFirstOrDefault(string userId , int productId);
        List<ShoppingCart> GetAllByUserIdWithIncludeProduct(string userId);
    }
}
