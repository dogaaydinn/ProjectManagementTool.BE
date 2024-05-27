using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using DataAccess.Repositories.Abstract.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace DataAccess.Repositories.Concrete.EntityFramework.ProjectManagement;

public class EfTeamDal : EfEntityRepository<Team, EfDbContext>, ITeamDal
{
}