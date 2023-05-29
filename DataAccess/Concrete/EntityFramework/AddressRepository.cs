
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class AddressRepository : EfEntityRepositoryBase<Address, ProjectDbContext>, IAddressRepository
    {
        public AddressRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
