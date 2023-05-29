
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ActivityRepository : EfEntityRepositoryBase<Activity, ProjectDbContext>, IActivityRepository
    {
        public ActivityRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
