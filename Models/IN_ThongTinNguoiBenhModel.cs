using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_ThongTinNguoiBenh
    {
        [Key]
        public int IdIN_ThongTinNguoiBenh { get; set; }
        public string? GioiTinh { get; set; }
        public int? Tuoi {get; set;}

        [Required]
        public string? SoDienThoai { get; set; } 
        public int? SoNgayNamVien { get; set; } 
        public bool? CoSuDungBHYT { get; set; } 
        
        
    }
}
