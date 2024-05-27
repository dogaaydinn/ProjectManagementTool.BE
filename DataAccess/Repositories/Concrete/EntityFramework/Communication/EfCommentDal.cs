using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using DataAccess.Repositories.Abstract.Communication;
using Domain.Entities.Communication;

namespace DataAccess.Repositories.Concrete.EntityFramework.Communication;

public class EfCommentDal : EfEntityRepository<Comment, EfDbContext>, ICommentDal
{
}