using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_ThongTinYKienKhac
    {
        [Key]
        public int IdIN_ThongTinYKienKhac { get; set; }
        public int? PhanTramMongDoi { get; set; }
        public string? QuayLaiVaGioiThieu { get; set; }
        public string? YKienKhac {get; set; }
        public DateTime NgayTao { get; set; } = DateTime.UtcNow; // Sử dụng giờ UTC
        public int IdIN_ThongTinNguoiBenh { get; set; }
        [ForeignKey("IdIN_ThongTinNguoiBenh")]
        public required IN_ThongTinNguoiBenh ThongTinNguoiBenh { get; set; }
        
    }
}
