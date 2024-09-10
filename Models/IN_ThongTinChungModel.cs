using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_ThongTinChung
    {
        [Key]
        public int IdIN_ThongTinChung { get; set; }
        public string? TenBenhVien { get; set; }
        public DateOnly NgayDienPhieu { get; set; } // Sử dụng giờ UTC
        public string? NguoiTraLoi { get; set; }
        public string? TenKhoa { get; set; }
        public string? MaKhoa { get; set; }
        public int IdIN_ThongTinNguoiBenh { get; set; }
        [ForeignKey("IdIN_ThongTinNguoiBenh")]
        public IN_ThongTinNguoiBenh? ThongTinNguoiBenh { get; set; }

    }
}
