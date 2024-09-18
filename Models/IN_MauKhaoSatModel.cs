using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace feedBackMvc.Models
{
    public class IN_MauKhaoSat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdIN_MauKhaoSat { get; set; }
        public DateOnly NgayTao { get; set; }  // Sử dụng giờ UTC
        public string? TenMauKhaoSat { get; set; }
        public string[]? NhomCauHoiKhaoSat { get; set; } // Thay đổi từ Title thành Titles
        public string[]? CauHoiKhaoSat { get; set; } // Thay đổi từ Question thành Questions
        public int idAdmin { get; set; }
        [ForeignKey("idAdmin")]
        public ICollection<IN_DanhGia>? DanhGia { get; set; }
        public Admins? admins { get; set; }
        public DateOnly? NgayBatDau { get; set; }
        public DateOnly? NgayKetThuc { get; set; }
        public bool? TrangThai { get; set; }
        public int? SoluongKhaoSat { get; set; }
        public bool? HienThi { get; set; }
        public bool? Xoa { get; set; }
        public double[]? MucQuanTrong { get; set; }

    }
}
