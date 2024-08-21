using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_NhomCauHoiKhaoSat
    {
        [Key]
        public int IdIN_NhomCauHoiKhaoSat { get; set; }
        [Required]
        public char? TieuDe { get; set; }
        [Required]
        public string? NoiDung { get; set; } 
        public int idAdmin { get; set; }
        [ForeignKey("idAdmin")]
        public Admins admins{ get; set; }
        
    }
}
