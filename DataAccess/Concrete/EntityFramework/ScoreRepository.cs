
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ScoreRepository : EfEntityRepositoryBase<Score, ProjectDbContext>, IScoreRepository
    {
        public ScoreRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
