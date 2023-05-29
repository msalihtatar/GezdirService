
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class TransportationRepository : EfEntityRepositoryBase<Transportation, ProjectDbContext>, ITransportationRepository
    {
        public TransportationRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
