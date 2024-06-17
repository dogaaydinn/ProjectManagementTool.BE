using Core.Utils.DI.Abstract;
using Core.Utils.Seed.Abstract;
using DataAccess.Context.EntityFramework;
using DataAccess.Repositories.Abstract.Association;
using DataAccess.Repositories.Abstract.Communication;
using DataAccess.Repositories.Abstract.ProjectManagement;
using DataAccess.Repositories.Abstract.TaskManagement;
using DataAccess.Repositories.Abstract.UserManagement;
using DataAccess.Repositories.Concrete.EntityFramework.Association;
using DataAccess.Repositories.Concrete.EntityFramework.Communication;
using DataAccess.Repositories.Concrete.EntityFramework.ProjectManagement;
using DataAccess.Repositories.Concrete.EntityFramework.TaskManagement;
using DataAccess.Repositories.Concrete.EntityFramework.UserManagement;
using DataAccess.Utils.Seed.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.DependencyResolvers;

public class DataAccessModule : IDependencyInjectionModule
{
    public void Load(IServiceCollection services)
    {
        #region DbContext

        services.AddDbContext<EfDbContext>();

        #endregion DbContext

        #region Repositories

        services.AddScoped<IProjectDal, EfProjectDal>();
        services.AddScoped<ITeamDal, EfTeamDal>();
        services.AddScoped<ITeamProjectDal, EfTeamProjectDal>();
        services.AddScoped<IDutyDal, EfDutyDal>();
        services.AddScoped<IUserDal, EfUserDal>();
        services.AddScoped<IUserDutyDal, EfUserDutyDal>();
        services.AddScoped<ICommentDal, EfCommentDal>();
        services.AddScoped<IDutyDal, EfDutyDal>();
        services.AddScoped<IUserTeamDal, EfUserTeamDal>();

        #endregion Repositories

        #region Utils

        services.AddScoped(typeof(ISeeder), typeof(EfSeeder));

        #endregion Utils
    }
}