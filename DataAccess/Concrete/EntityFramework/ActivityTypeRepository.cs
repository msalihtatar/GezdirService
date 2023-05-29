
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ActivityTypeRepository : EfEntityRepositoryBase<ActivityType, ProjectDbContext>, IActivityTypeRepository
    {
        public ActivityTypeRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
