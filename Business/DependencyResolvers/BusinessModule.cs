using Business.Services.Auth.Abstract;
using Business.Services.Auth.Concrete;
using Business.Services.Communication.Abstract;
using Business.Services.Communication.Concrete;
using Business.Services.DutyManagement.Abstract;
using Business.Services.DutyManagement.Concrete;
using Business.Services.ProjectManagement.Abstract;
using Business.Services.ProjectManagement.Concrete;
using Core.Utils.DI.Abstract;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolvers;

public class BusinessModule : IDependencyInjectionModule
{
    public void Load(IServiceCollection services)
    {
        # region Communication

        services.AddScoped<IMailingService, MailingManager>();
        services.AddScoped<ICommentService, CommentManager>();
        # endregion Communication
        
        # region DutyManagement
        services.AddScoped<IDutyService, DutyManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ILabelService, LabelManager>();
        
        # endregion DutyManagement
        
        # region ProjectManagement
        
        services.AddScoped<IProjectService, ProjectManager>();
        services.AddScoped<ITeamService, TeamManager>();
        
        # endregion ProjectManagement

        #region Auth

        services.AddScoped<IAuthService, AuthManager>();

        #endregion
        
        # region FluentValidation

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(typeof(BusinessModule).Assembly);

        # endregion FluentValidation
    }
}