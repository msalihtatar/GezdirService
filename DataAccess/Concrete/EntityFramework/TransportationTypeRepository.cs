
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class TransportationTypeRepository : EfEntityRepositoryBase<TransportationType, ProjectDbContext>, ITransportationTypeRepository
    {
        public TransportationTypeRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
