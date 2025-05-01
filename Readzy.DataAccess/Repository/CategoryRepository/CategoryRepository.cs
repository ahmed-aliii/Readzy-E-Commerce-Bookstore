using Microsoft.Identity.Client;
using Readzy.DataAccess.Data;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.CategoryRepository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private readonly ReadzyContext context;
        public CategoryRepository(ReadzyContext context):base(context) 
        {
            this.context = context;
        }

        public Category GetById(int? id)
        {
            return context.Categories.FirstOrDefault(cat => cat.Id == id);
        }

        public Category IsUnique(int displayOrder, int Id)
        {
            return context.Categories.FirstOrDefault(cat => cat.DisplayOrder == displayOrder && cat.Id != Id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Category category)
        {
            context.Categories.Update(category);
        }

    }
}
