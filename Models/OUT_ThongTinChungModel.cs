using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class OUT_ThongTinChung
    {
        [Key]
        public int IdOUT_ThongTinChung { get; set; }
        public string? TenBenhVien { get; set; }
        public DateTime NgayDienPhieu { get; set; } = DateTime.UtcNow; // Sử dụng giờ UTC
        public string? NguoiTraLoi { get; set; }
        public string? TenKhoa { get; set; }
        public string? MaKhoa { get; set; }
        public int IdOUT_ThongTinNguoiBenh { get; set; }
        [ForeignKey("IdOUT_ThongTinNguoiBenh")]
        public OUT_ThongTinNguoiBenh? ThongTinNguoiBenh { get; set; }

    }
}
