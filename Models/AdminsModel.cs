using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
namespace feedBackMvc.Models{
public class Admins 
{
    [Key]
    public int idAdmin { get; set; }
    public string? Ten { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string MatKhau { get; set; } 
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Thay thế PasswordHasher của Identity
    private readonly PasswordHasher<Admins> _hasher = new PasswordHasher<Admins>();

    public void SetPassword(string password)
    {
        MatKhau = _hasher.HashPassword(this, password);
    }

    public bool VerifyPassword(string password)
    {
        var result = _hasher.VerifyHashedPassword(this, MatKhau, password);
        return result == PasswordVerificationResult.Success;
    }
}


}