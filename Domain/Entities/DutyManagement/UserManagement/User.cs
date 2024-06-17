using System.ComponentModel.DataAnnotations;
using Core.Domain.Abstract;
using Domain.Entities.Communication;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.DutyManagement.UserManagement;

public class User : EntityBase
{
    [StringLength(127)] public string Username { get; set; }
    [StringLength(127)] public string FirstName { get; set; }
    [StringLength(127)] public string LastName { get; set; }
    [StringLength(127)] public string? ProfilePicture { get; set; }
    [StringLength(127)] public string Email { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    [StringLength(15)] public string Role { get; set; }

    public virtual ICollection<Duty>
        AssignedDuties { get; set; } // Navigation property for many-to-many relationship with Task

    public virtual ICollection<Team>
        ParticipatedTeams { get; set; } // Navigation property for many-to-many relationship with Team

    public virtual ICollection<Project>
        ManagedProjects { get; set; } // Navigation property for one-to-many relationship with Project

    public virtual ICollection<Comment>
        Comments { get; set; } // Navigation property for one-to-many relationship with Comment

    [StringLength(127)] public string? EmailVerificationCode { get; set; }
    [StringLength(127)] public string PhoneNumber { get; set; } = null!;
    [StringLength(6)] public string? LoginVerificationCode { get; set; }
    public string? ActiveToken { get; set; }
    public DateTime? LoginVerificationCodeExpiration { get; set; }
    public bool UseMultiFactorAuthentication { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneNumberVerified { get; set; }
    public DateTime LastLoginTime { get; set; }

    public virtual ICollection<Duty> ReportedDuties { get; set; }
    public string? ResetPasswordCode { get; set; }
    public DateTime? ResetPasswordCodeExpiration { get; set; }
}