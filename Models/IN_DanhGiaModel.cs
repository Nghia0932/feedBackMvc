using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_DanhGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdIN_DanhGia { get; set; }
        public DateOnly? NgayDanhGia { get; set; } // Sử dụng giờ UTC
        public required int[] DanhGia { get; set; }
        public double[]? DanhGiaTong { get; set; }
        public int IdIN_MauKhaoSat { get; set; }
        public IN_MauKhaoSat? MauKhaoSat { get; set; }
        public int IdIN_ThongTinNguoiBenh { get; set; }
        public IN_ThongTinNguoiBenh? ThongTinNguoiBenh { get; set; }


    }
}
