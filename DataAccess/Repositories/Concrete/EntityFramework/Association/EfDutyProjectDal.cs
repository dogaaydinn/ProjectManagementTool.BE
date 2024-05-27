using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using DataAccess.Repositories.Abstract.Association;
using Domain.Entities.Association;

namespace DataAccess.Repositories.Concrete.EntityFramework.Association;

public class EfDutyProjectDal : EfEntityRepository<TeamProject, EfDbContext>, IDutyProjectDal
{
}