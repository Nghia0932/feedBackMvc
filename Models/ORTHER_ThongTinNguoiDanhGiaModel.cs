using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class ORTHER_ThongTinNguoiDanhGia
    {
        [Key]
        public int IdORTHER_ThongTinNguoiDanhGia { get; set; }
        public string? GioiTinh { get; set; }
        public int? Tuoi { get; set; }

        [Required]
        public string? SoDienThoai { get; set; }
        public ICollection<ORTHER_DanhGia>? DanhGia { get; set; }

    }
}
