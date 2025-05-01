using Readzy.DataAccess.Data;
using Readzy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository.ApplicationUserRepository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ReadzyContext context;
        public ApplicationUserRepository(ReadzyContext context) : base(context)
        {
            this.context = context;
        }

        public ApplicationUser GetById(string id)
        {
            return context.ApplicationUsers.FirstOrDefault(au => au.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ApplicationUser ApplicationUser)
        {
            context.ApplicationUsers.Update(ApplicationUser);

        }
    }
}
