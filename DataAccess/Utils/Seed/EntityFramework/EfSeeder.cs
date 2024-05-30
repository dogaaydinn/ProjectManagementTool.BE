using Core.Constants;
using Core.Constants.Duty;
using Core.Constants.Project;
using Core.Utils.Hashing;
using Core.Utils.Seed.Abstract;
using DataAccess.Context.EntityFramework;
using Domain.Entities.Association;
using Domain.Entities.Communication;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Utils.Seed.EntityFramework;

public class EfSeeder : ISeeder
{
    public void Seed(IApplicationBuilder builder)
    {
        var context = builder.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<EfDbContext>();

        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();

        // If there is no data in the database, then seed the database
        if (context.Duties.Any()) 
            return;

        

        byte[] passwordHash = null;
        byte[] passwordSalt = null;

        HashingHelper.CreatePasswordHash("Man123456789.", out passwordHash, out passwordSalt);

        var manager = new User
        {
            Id = Guid.Empty,
            Username = "manager",
            FirstName = "manager",
            LastName = "manager",
            PhoneNumber = "05452977501",
            UseMultiFactorAuthentication = false,
            Email = "doga.aydin@arena.com.tr",
            EmailVerified = true,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };

        manager.CreatedUserId = manager.Id;
        context.Users.Add(manager);
        context.SaveChanges();

        #region Users
        HashingHelper.CreatePasswordHash("Adar123456789.", out passwordHash, out passwordSalt);
        var adar = new User
        {
            Username = "adar",
            FirstName = "adar",
            LastName = "sönmez",
            PhoneNumber = "05452977503",
            UseMultiFactorAuthentication = false,
            Email = "adar@testmail.com",
            EmailVerified = true,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Arda123456789.", out passwordHash, out passwordSalt);
        var arda = new User
        {
            Username = "arda",
            FirstName = "arda",
            LastName = "turan",
            PhoneNumber = "05452977502",
            UseMultiFactorAuthentication = false,
            Email = "arda@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Onur123456789.", out passwordHash, out passwordSalt);
        var onur = new User
        {
            Username = "onur",
            FirstName = "onur",
            LastName = "kılınc",
            PhoneNumber = "05452977504",
            UseMultiFactorAuthentication = false,
            Email = "onur@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Doga123456789.", out passwordHash, out passwordSalt);
        var doga = new User
        {
            Username = "doga",
            FirstName = "doga",
            LastName = "aydin",
            PhoneNumber = "05452977505",
            UseMultiFactorAuthentication = false,
            Email = "doga@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Tuğba123456789.", out passwordHash, out passwordSalt);
        var tugba = new User
        {
            Username = "emre",
            FirstName = "emre",
            LastName = "yilmaz",
            PhoneNumber = "05452977506",
            UseMultiFactorAuthentication = false,
            Email = "emre@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Barıs123456789.", out passwordHash, out passwordSalt);
        var barıs = new User
        {
            Username = "baris",
            FirstName = "baris",
            LastName = "yilmaz",
            PhoneNumber = "05452977507",
            UseMultiFactorAuthentication = false,
            Email = "barıs@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Mesut123456789.", out passwordHash, out passwordSalt);
        var mesut = new User
        {
            Username = "mesut",
            FirstName = "mesut",
            LastName = "ada",
            PhoneNumber = "05452977508",
            UseMultiFactorAuthentication = false,
            Email = "mesut@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Fatih123456789.", out passwordHash, out passwordSalt);
        var fatih = new User
        {
            Username = "fatih",
            FirstName = "fatih",
            LastName = "acar",
            PhoneNumber = "05452977509",
            UseMultiFactorAuthentication = false,
            Email = "fatih@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Elif123456789.", out passwordHash, out passwordSalt);
        var elif = new User
        {
            Username = "elif",
            FirstName = "elif",
            LastName = "turan",
            PhoneNumber = "05452977510",
            UseMultiFactorAuthentication = false,
            Email = "elif@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Emre123456789.", out passwordHash, out passwordSalt);
        var emre = new User
        {
            Username = "emre",
            FirstName = "emre",
            LastName = "san",
            PhoneNumber = "05452977511",
            UseMultiFactorAuthentication = false,
            Email = "emre@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Manager,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Yusuf123456789.", out passwordHash, out passwordSalt);
        var yusuf = new User
        {
            Username = "yusuf",
            FirstName = "yusuf",
            LastName = "ege",
            PhoneNumber = "05452977512",
            UseMultiFactorAuthentication = false,
            Email = "yusuf@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Manager,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Aslı123456789.", out passwordHash, out passwordSalt);
        var aslı = new User
        {
            Username = "aslı",
            FirstName = "aslı",
            LastName = "kaya",
            PhoneNumber = "05452977513",
            UseMultiFactorAuthentication = false,
            Email = "aslı@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Mehmet123456789.", out passwordHash, out passwordSalt);
        var mehmet = new User
        {
            Username = "mehmet",
            FirstName = "mehmet",
            LastName = "kara",
            PhoneNumber = "05452977514",
            UseMultiFactorAuthentication = false,
            Email = "mehmet@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Ali123456789.", out passwordHash, out passwordSalt);
        var ali = new User
        {
            Username = "ali",
            FirstName = "ali",
            LastName = "aydin",
            PhoneNumber = "05452977515",
            UseMultiFactorAuthentication = false,
            Email = "ali@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Ufuk123456789.", out passwordHash, out passwordSalt);
        var ufuk = new User
        {
            Username = "ufuk",
            FirstName = "ufuk",
            LastName = "demir",
            PhoneNumber = "05452977516",
            UseMultiFactorAuthentication = false,
            Email = "ufuk@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Manager,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Ahmet123456789.", out passwordHash, out passwordSalt);
        var ahmet = new User
        {
            Username = "ahmet",
            FirstName = "ahmet",
            LastName = "ay",
            PhoneNumber = "05452977517",
            UseMultiFactorAuthentication = false,
            Email = "ahmet@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Selim123456789.", out passwordHash, out passwordSalt);
        var selim = new User
        {
            Username = "selim",
            FirstName = "selim",
            LastName = "kaya",
            PhoneNumber = "05452977518",
            UseMultiFactorAuthentication = false,
            Email = "selim@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Assignee,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Yasin123456789.", out passwordHash, out passwordSalt);
        var yasin = new User
        {
            Username = "yasin",
            FirstName = "yasin",
            LastName = "ozel",
            PhoneNumber = "05452977519",
            UseMultiFactorAuthentication = false,
            Email = "yasin@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Manager,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Burak123456789.", out passwordHash, out passwordSalt);
        var burak = new User
        {
            Username = "burak",
            FirstName = "burak",
            LastName = "tas",
            PhoneNumber = "05452977520",
            UseMultiFactorAuthentication = false,
            Email = "burak@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.Manager,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Cem123456789.", out passwordHash, out passwordSalt);
        var cem = new User
        {
            Username = "cem",
            FirstName = "cem",
            LastName = "yılmaz",
            PhoneNumber = "05452977521",
            UseMultiFactorAuthentication = false,
            Email = "cem@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        HashingHelper.CreatePasswordHash("Deniz123456789.", out passwordHash, out passwordSalt);
        var deniz = new User
        {
            Username = "deniz",
            FirstName = "deniz",
            LastName = "akkaya",
            PhoneNumber = "05452977522",
            UseMultiFactorAuthentication = false,
            Email = "deniz@testmail.com",
            EmailVerified = false,
            PhoneNumberVerified = true,
            Role = UserRoles.TeamMember,
            PasswordHash = passwordHash,
            PasswordSalt =passwordSalt
        };
        
        context.Users.AddRange(adar, arda, onur, doga, tugba, barıs, mesut, fatih, elif, emre, yusuf, aslı, mehmet, ali, ufuk, ahmet, selim, yasin, burak, cem, deniz);
        context.SaveChanges();

        #endregion
        #region Teams

        var ai = new Team // DDD manager olmasın
        {
            Name = "AI",
            CreatedUserId = tugba.Id,
            ManagerId = tugba.Id,
            Description = "AI Team"
        };
        var productDevelopment = new Team
        {
            Name = "Product Development",
            CreatedUserId = adar.Id,
            ManagerId = arda.Id,
            Description = "Product Development Team"
        };
        var projectManagement = new Team
        {
            Name = "Project Management",
            CreatedUserId = doga.Id,
            ManagerId = adar.Id,
            Description = "Project Management Team"
        };
        var humanResources = new Team
        {
            Name = "Human Resources",
            CreatedUserId = ahmet.Id,
            ManagerId = emre.Id,
            Description = "Human Resources Team"
        };
        var marketing = new Team
        {
            Name = "Marketing",
            CreatedUserId = elif.Id,
            ManagerId = yusuf.Id,
            Description = "Marketing Team"
        };
        var operations = new Team
        {
            Name = "Operations",
            CreatedUserId = onur.Id,
            ManagerId = arda.Id,
            Description = "Operations Team"
        };
        var finance = new Team
        {
            Name = "Finance",
            CreatedUserId = mehmet.Id,
            ManagerId = mehmet.Id,
            Description = "Finance Team"
        };
        var sales = new Team
        {
            Name = "Sales",
            CreatedUserId = adar.Id,
            ManagerId =doga.Id,
            Description = "Sales Team"
        };
        var customerRelations = new Team
        {
            Name = "Customer Relations",
            CreatedUserId = emre.Id,
            ManagerId = yusuf.Id,
            Description = "Customer Relations Team"
        };
        var legal = new Team
        {
            Name = "Legal",
            CreatedUserId = aslı.Id,
            ManagerId = ahmet.Id,
            Description = "Legal Team"
        };
        var qualityAssurance = new Team
        {
            Name = "Quality Assurance",
            CreatedUserId = selim.Id,
            ManagerId = deniz.Id,
            Description = "Quality Assurance Team"
        };
        var researchAndDevelopment = new Team
        {
            Name = "Research and Development",
            CreatedUserId = fatih.Id,
            ManagerId = cem.Id,
            Description = "Research and Development Team"
        };
        var supplyChain = new Team
        {
            Name = "Supply Chain",
            CreatedUserId = burak.Id,
            ManagerId = mehmet.Id,
            Description = "Supply Chain Team"
        };
        var logistics = new Team
        {
            Name = "Logistics",
            CreatedUserId = elif.Id,
            ManagerId = tugba.Id,
            Description = "Logistics Team"
        };
        var warehousing = new Team
        {
            Name = "Warehousing",
            CreatedUserId = onur.Id,
            ManagerId = ufuk.Id,
            Description = "Warehousing Team"
        };
        var informationTechnology = new Team
        {
            Name = "Information Technology",
            CreatedUserId = adar.Id,
            ManagerId = adar.Id,
            Description = "Information Technology Team"
        };
        var engineering = new Team
        {
            Name = "Engineering",
            CreatedUserId = doga.Id,
            ManagerId = adar.Id,
            Description = "Engineering Team"
        };
        var design = new Team
        {
            Name = "Design",
            CreatedUserId = selim.Id,
            ManagerId = doga.Id,
            Description = "Design Team"
        };
        var communications = new Team
        {
            Name = "Communications",
            CreatedUserId = mehmet.Id,
            ManagerId = tugba.Id,
            Description = "Communications Team"
        };
        var financeAndAccounting = new Team
        {
            Name = "Finance and Accounting",
            CreatedUserId = emre.Id,
            ManagerId = onur.Id,
            Description = "Finance and Accounting Team"
        };
        var research = new Team
        {
            Name = "Research",
            CreatedUserId = doga.Id,
            ManagerId = doga.Id,
            Description = "Research Team"
        };
        context.Teams.AddRange(ai, productDevelopment, projectManagement, humanResources, marketing, operations, finance, sales, customerRelations, legal, qualityAssurance, researchAndDevelopment, supplyChain, logistics, warehousing, informationTechnology, engineering, design, communications, financeAndAccounting, research);
        context.SaveChanges();
        #endregion
        #region Projects
        // DDD looks good

        var project1 = new Project
        {
            Name = "Project 1",
            Description = "Project 1 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Low,
            ManagerId = manager.Id,

        };
        var project2 = new Project
        {
            Name = "Project 2",
            Description = "Project 2 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Medium,
            ManagerId = manager.Id,

        };
        var project3 = new Project
        {
            Name = "Project 3",
            Description = "Project 3 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.High,
            ManagerId = manager.Id,
        };
        var project4 = new Project
        {
            Name = "Project 4",
            Description = "Project 4 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.High,
            ManagerId = manager.Id,
        };
        var project5 = new Project
        {
            Name = "Project 5",
            Description = "Project 5 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Low,
            ManagerId = manager.Id,
        };
        var project6 = new Project
        {
            Name = "Project 6",
            Description = "Project 6 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Low,
            ManagerId = manager.Id,
        };
        var project7 = new Project
        {
            Name = "Project 7",
            Description = "Project 7 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Low,
            ManagerId = manager.Id,
        };
        var project8 = new Project
        {
            Name = "Project 8",
            Description = "Project 8 Description",
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(30),
            Status = ProjectStatus.Active,
            Priority = Priority.Low,
            ManagerId = manager.Id,
        };

        context.Projects.AddRange(project1, project2, project3, project4, project5, project6, project7, project8);
        context.SaveChanges();

        #endregion
        #region Duties
        //write duty seed data
        var duty1 = new Duty
        {
            Title = "Duty 1",
            Description = "Duty 1 Description",
            DutyType = DutyType.ToDo,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Low,
            ProjectId = project1.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.ToDo
            
        };
        var duty2 = new Duty
        {
            Title = "Duty 2",
            Description = "Duty 2 Description",
            DutyType = DutyType.InProgress,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Medium,
            ProjectId = project2.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.InProgress
        };
        var duty3 = new Duty
        {
            Title = "Duty 3",
            Description = "Duty 3 Description",
            DutyType = DutyType.Completed,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.High,
            ProjectId = project3.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.Completed
        };
        var duty4 = new Duty
        {
            Title = "Duty 4",
            Description = "Duty 4 Description",
            DutyType = DutyType.Canceled,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.High,
            ProjectId = project4.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.Canceled
        };
        var duty5 = new Duty
        {
            Title = "Duty 5",
            Description = "Duty 5 Description",
            DutyType = DutyType.New,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Low,
            ProjectId = project5.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.InReview
        };
        var duty6 = new Duty
        {
            Title = "Duty 6",
            Description = "Duty 6 Description",
            DutyType = DutyType.InReview,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Low,
            ProjectId = project6.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.InReview
        };
        var duty7 = new Duty
        {
            Title = "Duty 7",
            Description = "Duty 7 Description",
            DutyType = DutyType.Planned,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Low,
            ReporterId = manager.Id,
            ProjectId = project7.Id
        };
        var duty8 = new Duty
        {
            Title = "Duty 8",
            Description = "Duty 8 Description",
            DutyType = DutyType.Planned,
            DueDate = DateTime.Now.AddDays(30),
            Priority = Priority.Low,
            ProjectId = project8.Id,
            ReporterId = manager.Id,
            Status = DutyStatus.ToDo
        };
        context.Duties.AddRange(duty1, duty2, duty3, duty4, duty5, duty6, duty7, duty8); 
        context.SaveChanges();
        
        #endregion
        #region UserDuties
        //write user duties seed data
        
        var userDuty1 = new UserDuty
        {
            UserId = adar.Id,
            DutyId = duty1.Id
        };
        var userDuty2 = new UserDuty
        {
            UserId = arda.Id,
            DutyId = duty2.Id
        };
        var userDuty3 = new UserDuty
        {
            UserId = onur.Id,
            DutyId = duty3.Id
        };
        var userDuty4 = new UserDuty
        {
            UserId = barıs.Id,
            DutyId = duty4.Id
        };
        var userDuty5 = new UserDuty
        {
            UserId = tugba.Id,
            DutyId = duty5.Id
        };
        var userDuty6 = new UserDuty
        {
            UserId = arda.Id,
            DutyId = duty6.Id
        };
        var userDuty7 = new UserDuty
        {
            UserId = adar.Id,
            DutyId = duty7.Id
        };
        var userDuty8 = new UserDuty
        {
            UserId = onur.Id,
            DutyId = duty8.Id
        };
        context.UserDuties.AddRange(userDuty1, userDuty2, userDuty3, userDuty4, userDuty5, userDuty6, userDuty7, userDuty8);
        context.SaveChanges();
        #endregion
        #region TeamProjects

        var tp1 = new TeamProject
        {
            TeamId = ai.Id,
            ProjectId = project1.Id
        }; // DDD dışarı çıkar
        var tp2 = new TeamProject
        {
            TeamId = productDevelopment.Id,
            ProjectId = project2.Id
        };
        var tp3 = new TeamProject
        {
            TeamId = projectManagement.Id,
            ProjectId = project3.Id
        };
        var tp4 = new TeamProject
        {
            TeamId = humanResources.Id,
            ProjectId = project4.Id
        };
        var tp5 = new TeamProject
        {
            TeamId = marketing.Id,
            ProjectId = project5.Id
        };
        var tp6 = new TeamProject
        {
            TeamId = operations.Id,
            ProjectId = project6.Id
        };
        var tp7 = new TeamProject
        {
            TeamId = finance.Id,
            ProjectId = project7.Id
        };
        var tp8 = new TeamProject
        {
            TeamId = sales.Id,
            ProjectId = project8.Id
        };
        context.TeamProjects.AddRange(tp1, tp2, tp3, tp4, tp5, tp6, tp7, tp8);
        context.SaveChanges();

        #endregion
        #region UserTeams

        var userTeam = new UserTeam
        {
            UserId = doga.Id,
            TeamId = ai.Id
        };
        var userTeam1 = new UserTeam
        {
            UserId = doga.Id,
            TeamId = productDevelopment.Id
        };
        var userTeam2 = new UserTeam
        {
            UserId = adar.Id,
            TeamId = productDevelopment.Id
        };
        var userTeam3 = new UserTeam
        {
            UserId = barıs.Id,
            TeamId = projectManagement.Id
        };
        var userTeam4 = new UserTeam
        {
            UserId = barıs.Id,
            TeamId = humanResources.Id
        };
        var userTeam5 = new UserTeam
        {
            UserId = tugba.Id,
            TeamId = marketing.Id
        };
        var userTeam6 = new UserTeam
        {
            UserId = arda.Id,
            TeamId = marketing.Id
        };
        var userTeam7 = new UserTeam
        {
            UserId = adar.Id,
            TeamId = operations.Id
        };
        var userTeam8 = new UserTeam
        {
            UserId = onur.Id,
            TeamId = finance.Id
        };
        var userTeam9 = new UserTeam
        {
            UserId = tugba.Id,
            TeamId = sales.Id
        };
        context.UserTeams.AddRange(userTeam, userTeam1, userTeam2, userTeam3, userTeam4, userTeam5, userTeam6, userTeam7, userTeam8, userTeam9);
        context.SaveChanges();


        #endregion
        #region Labels
        var label1 = new Label
        {
            Name = "Label 1",
            Color = LabelColors.White,
            Description = "Label 1 Description"
        };
        var label2 = new Label
        {
            Name = "Label 2",
            Color = LabelColors.Black,
            Description = "Label 2 Description"
        };
        var label3 = new Label
        {
            Name = "Label 3",
            Color = LabelColors.Red,
            Description = "Label 3 Description"
        };
        var label4 = new Label
        {
            Name = "Label 4",
            Color = LabelColors.Green,
            Description = "Label 4 Description"
        };
        var label5 = new Label
        {
            Name = "Label 5",
            Color = LabelColors.Blue,
            Description = "Label 5 Description"
        };
        var label6 = new Label
        {
            Name = "Label 6",
            Color = LabelColors.Yellow,
            Description = "Label 6 Description"
        };
        var label7 = new Label
        {
            Name = "Label 7",
            Color = LabelColors.Purple,
            Description = "Label 7 Description"
        };
        var label8 = new Label
        {
            Name = "Label 8",
            Color = LabelColors.Magenta,
            Description = "Label 8 Description"
        };
        var label9 = new Label
        {
            Name = "Label 9",
            Color = LabelColors.Orange,
            Description = "Label 9 Description"
        };
        var label10 = new Label
        {
            Name = "Label 10",
            Color = LabelColors.Pink,
            Description = "Label 10 Description"
        };
        context.Labels.AddRange(label1, label2, label3, label4, label5, label6, label7, label8, label9, label10);
        context.SaveChanges();
        #endregion
        #region Comment
        var comment1 = new Comment
        {
            AuthorId = adar.Id,
            DutyId = duty1.Id,
            Text = "Comment 1",
            
            CreatedAt = DateTime.Now
        };
        var comment2 = new Comment
        {
            AuthorId = arda.Id,
            DutyId = duty2.Id,
            Text = "Comment 2",
            CreatedAt = DateTime.Now
        };
        var comment3 = new Comment
        {
            AuthorId = onur.Id,
            DutyId = duty3.Id,
            Text = "Comment 3",
            CreatedAt = DateTime.Now
        };
        var comment4 = new Comment
        {
            AuthorId = barıs.Id,
            DutyId = duty4.Id,
            Text = "Comment 4",
            CreatedAt = DateTime.Now
        };
        var comment5 = new Comment
        {
            AuthorId = tugba.Id,
            DutyId = duty5.Id,
            Text = "Comment 5",
            CreatedAt = DateTime.Now
        };
        var comment6 = new Comment
        {
            AuthorId = arda.Id,
            DutyId = duty6.Id,
            Text = "Comment 6",
            CreatedAt = DateTime.Now
        };
        var comment7 = new Comment
        {
            AuthorId = adar.Id,
            DutyId = duty7.Id,
            Text = "Comment 7",
            CreatedAt = DateTime.Now
        };
        var comment8 = new Comment
        {
            AuthorId = onur.Id,
            DutyId = duty8.Id,
            Text = "Comment 8",
            CreatedAt = DateTime.Now
        };
        context.Comments.AddRange(comment1, comment2, comment3, comment4, comment5, comment6, comment7, comment8);
        context.SaveChanges();
        #endregion
    }

    #region Seed Data Nedir?

    //Seed Data'lar migration'ların dışında eklenmesi ve değiştirilmesi beklenmeyen durumlar için kullanılan bir özelliktir!

    #endregion

    #region Data Seeding Nedir?

//EF Core ile inşa edilen veritabanı içerisinde veritabanı nesneleri olabileceği gibi verilerinde migrate sürecinde üretilmesini isteyebiliriz.
//İşte bu ihtiyaca istinaden Seed Data özelliği ile EF Core üzerinden migrationlarda veriler oluşturabilir ve migrate ederken bu verileri hedef tablolarımıza basabiliriz.
//Seed Data'lar, migrate süreçlerinde hazır verileri tablolara basabilmek için bunları yazılım kısmında tutmamızı gerektirmektedirler. Böylece bu veriler üzerinde veritabanı seviyesinde istenilen manipülasyonlar gönül rahatlığıyla gerçekleştirilebilmektedir.

//Data Seeding özelliği şu noktalarda kullanılabilir;
//Test için geçici verilere ihtiyaç varsa,
//Asp.NET Core'daki Identity yapılanmasındaki roller gibi static değerler de tutulabilir.
//Yazılım için temel konfigürasyonel değerler.

    #endregion

    #region Seed Data Ekleme

//OnModelCreating metodu içerisinde Entity fonksiyonundan sonra çağrılan HasData fonksiyonu ilgili entitye karşılık Seed Data'ları eklememizi sağlayan bir fonksiyondur.

//PK değerlerinin manuel olarak bildirilmesi/verilmesi gerekmektedir. Neden diye sorarsanız eğer, ilişkisel verileri de Seed Datalarla üretebilmek için...

    #endregion

    #region İlişkisel Tablolar İçin Seed Data Ekleme

//İlişkisel senaryolarda dependent table'a veri eklerken foreign key kolonunun propertysi varsa eğer ona ilişkisel değerini vererek ekleme işlemini yapıyoruz.

    #endregion

    #region Seed Datanın Primary Key'ini Değiştirme

//Eğer ki migrate edilen herhangi bir seed datanın sonrasında PK'i değiştirilirse bu datayla varsa ilişkisel başka veriler onlara cascade davranışı sergilenecektir.

    #endregion
}