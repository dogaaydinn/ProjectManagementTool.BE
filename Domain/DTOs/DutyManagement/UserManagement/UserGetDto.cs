using System.ComponentModel.DataAnnotations;
using Core.Domain.Abstract;

namespace Domain.DTOs.DutyManagement.UserManagement;

public class UserGetDto : IDto
{
    public Guid Id { get; set; } = default!;
    [StringLength(127)] public string Username { get; set; }
    [StringLength(127)] public string Email { get; set; }
    [StringLength(127)] public string Role { get; set; }
    [StringLength(127)] public string PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePhoto { get; set; }
    public bool UseMultiFactorAuthentication { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool EmailVerified { get; set; }
    public bool PhoneNumberVerified { get; set; }
    public DateTime LastLoginTime { get; set; } = DateTime.Now;
}
