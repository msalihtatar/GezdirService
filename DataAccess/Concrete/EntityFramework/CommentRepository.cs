
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class CommentRepository : EfEntityRepositoryBase<Comment, ProjectDbContext>, ICommentRepository
    {
        public CommentRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
