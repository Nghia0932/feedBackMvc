using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class OUT_ThongTinNguoiBenh
    {
        [Key]
        public int IdOUT_ThongTinNguoiBenh { get; set; }
        public string? GioiTinh { get; set; }
        public int? Tuoi { get; set; }

        [Required]
        public string? SoDienThoai { get; set; }
        public int? SoNgayNamVien { get; set; }
        public int? KhoangCach { get; set; }
        public bool? CoSuDungBHYT { get; set; }
        public ICollection<OUT_DanhGia>? OUT_DanhGia { get; set; }

    }
}
