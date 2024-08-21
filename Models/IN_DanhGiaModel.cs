using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_DanhGia
    {
        [Key]
        public int IdIN_DanhGia { get; set; }
        public DateTime NgayDanhGia { get; set; } = DateTime.UtcNow; // Sử dụng giờ UTC
        public required int[] DanhGia { get; set;}

        public int IdIN_MauKhaoSat { get; set; }
        [ForeignKey("IdIN_MauKhaoSat")]
        public required IN_MauKhaoSat MauKhaoSat { get; set; }
        public int IdIN_ThongTinNguoiBenh { get; set; }
        [ForeignKey("IdIN_ThongTinNguoiBenh")]
        public required IN_ThongTinNguoiBenh ThongTinNguoiBenh { get; set; }
        
    }
}
