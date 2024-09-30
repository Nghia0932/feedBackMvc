using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class ORTHER_DanhGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdORTHER_DanhGia { get; set; }
        public DateOnly? NgayDanhGia { get; set; } // Sử dụng giờ UTC
        public required int[] DanhGia { get; set; }
        public double[]? DanhGiaTong { get; set; }
        public int IdORTHER_MauKhaoSat { get; set; }
        public ORTHER_MauKhaoSat? MauKhaoSat { get; set; }
        public int IdORTHER_ThongTinNguoiDanhGia { get; set; }
        public ORTHER_ThongTinNguoiDanhGia? ThongTinNguoiDanhGia { get; set; }


    }
}
