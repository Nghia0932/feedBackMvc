using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class OUT_MauKhaoSat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOUT_MauKhaoSat { get; set; }
        public DateOnly NgayTao { get; set; }
        public string? TenMauKhaoSat { get; set; }
        public string[]? NhomCauHoiKhaoSat { get; set; } // Thay đổi từ Title thành Titles
        public string[]? CauHoiKhaoSat { get; set; } // Thay đổi từ Question thành Questions
        public int idAdmin { get; set; }
        [ForeignKey("idAdmin")]
        public ICollection<OUT_DanhGia>? DanhGia { get; set; }
        public Admins? admins { get; set; }
        public DateOnly? NgayBatDau { get; set; }
        public DateOnly? NgayKetThuc { get; set; }
        public int? SoluongKhaoSat { get; set; }

    }
}
