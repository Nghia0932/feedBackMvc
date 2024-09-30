using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class ORTHER_ThongTinYKienKhac
    {
        [Key]
        public int IdORTHER_ThongTinYKienKhac { get; set; }
        public int? PhanTramMongDoi { get; set; }
        public string? QuayLaiVaGioiThieu { get; set; }
        public string? YKienKhac { get; set; }
        public DateOnly? NgayTao { get; set; } // Sử dụng giờ UTC
        public int IdORTHER_ThongTinNguoiDanhGia { get; set; }
        [ForeignKey("IdORTHER_ThongTinNguoiDanhGia")]
        public ORTHER_ThongTinNguoiDanhGia? ThongTinNguoiDanhGia { get; set; }

    }
}
