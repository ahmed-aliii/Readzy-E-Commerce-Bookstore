using Readzy.DataAccess.Repository.IRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.ProductRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        void Save();
        Product GetById(int? id);
        Product GetByIdWithInclude(int id);
    }
}
