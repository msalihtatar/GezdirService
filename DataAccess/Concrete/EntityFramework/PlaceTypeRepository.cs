
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class PlaceTypeRepository : EfEntityRepositoryBase<PlaceType, ProjectDbContext>, IPlaceTypeRepository
    {
        public PlaceTypeRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
