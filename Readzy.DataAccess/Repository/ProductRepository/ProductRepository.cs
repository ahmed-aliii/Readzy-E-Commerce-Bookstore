using Microsoft.EntityFrameworkCore;
using Readzy.DataAccess.Data;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.ProductRepository
{
    public class ProductRepository : Repository<Product>  , IProductRepository
    {
        private readonly ReadzyContext context;
        public ProductRepository(ReadzyContext context) : base(context)
        {
            this.context = context;
        }

        public Product GetById(int? id)
        {
            return context.Products.FirstOrDefault(pro => pro.Id == id);
        }

        public Product GetByIdWithInclude(int id)
        {
            return context.Products.Include(pro => pro.Category).FirstOrDefault(pro => pro.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
        }
    }
}
