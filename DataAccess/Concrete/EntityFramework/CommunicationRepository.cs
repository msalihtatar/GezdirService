
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class CommunicationRepository : EfEntityRepositoryBase<Communication, ProjectDbContext>, ICommunicationRepository
    {
        public CommunicationRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
