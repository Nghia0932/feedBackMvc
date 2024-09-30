using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
namespace feedBackMvc.Models
{
    public class Admins
    {
        [Key]
        public int idAdmin { get; set; }
        public string? Ten { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string MatKhau { get; set; }
        public int? Role { get; set; }
        public bool? Xoa { get; set; }
        public DateOnly CreatedDate { get; set; }
        public ICollection<IN_MauKhaoSat>? MauKhaoSats { get; set; }
        public ICollection<OUT_MauKhaoSat>? OUT_MauKhaoSats { get; set; }
        public ICollection<ORTHER_MauKhaoSat>? ORTHER_MauKhaoSats { get; set; }

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