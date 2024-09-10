using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class OUT_ThongTinYKienKhac
    {
        [Key]
        public int IdOUT_ThongTinYKienKhac { get; set; }
        public int? PhanTramMongDoi { get; set; }
        public string? QuayLaiVaGioiThieu { get; set; }
        public string? YKienKhac { get; set; }
        public DateOnly NgayTao { get; set; }// Sử dụng giờ UTC
        public int IdOUT_ThongTinNguoiBenh { get; set; }
        [ForeignKey("IdOUT_ThongTinNguoiBenh")]
        public required OUT_ThongTinNguoiBenh ThongTinNguoiBenh { get; set; }

    }
}
