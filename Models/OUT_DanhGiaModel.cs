using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class OUT_DanhGia
    {
        [Key]
        public int IdOUT_DanhGia { get; set; }
        public DateOnly NgayDanhGia { get; set; }  // Sử dụng giờ UTC
        public required int[] DanhGia { get; set; }

        public int IdOUT_MauKhaoSat { get; set; }
        public required OUT_MauKhaoSat MauKhaoSat { get; set; }
        public int IdOUT_ThongTinNguoiBenh { get; set; }
        public required OUT_ThongTinNguoiBenh ThongTinNguoiBenh { get; set; }

    }
}
