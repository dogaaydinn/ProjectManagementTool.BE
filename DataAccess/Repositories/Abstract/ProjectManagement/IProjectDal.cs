using Core.DataAccess.Abstract;
using Domain.Entities.ProjectManagement;

namespace DataAccess.Repositories.Abstract.ProjectManagement;

public interface IProjectDal : IEntityRepository<Project>
{
}