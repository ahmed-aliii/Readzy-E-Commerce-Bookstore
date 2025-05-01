using Readzy.DataAccess.Repository.IRepository;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.CategoryRepository
{
    //Extra Function for CategoryRepository 
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
        void Save();
        Category GetById(int? id);

        Category IsUnique(int displayOrder, int Id);
    }
}
