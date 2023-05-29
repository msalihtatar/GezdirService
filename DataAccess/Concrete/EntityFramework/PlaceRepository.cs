
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class PlaceRepository : EfEntityRepositoryBase<Place, ProjectDbContext>, IPlaceRepository
    {
        public PlaceRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
