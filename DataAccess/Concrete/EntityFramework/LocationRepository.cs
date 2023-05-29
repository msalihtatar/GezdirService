
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class LocationRepository : EfEntityRepositoryBase<Location, ProjectDbContext>, ILocationRepository
    {
        public LocationRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
