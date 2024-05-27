using Core.DataAccess.Abstract;
using Domain.Entities.DutyManagement.UserManagement;

namespace DataAccess.Repositories.Abstract.UserManagement;

public interface IUserDal : IEntityRepository<User>
{
}