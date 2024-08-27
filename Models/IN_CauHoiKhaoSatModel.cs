using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedBackMvc.Models
{
    public class IN_CauHoiKhaoSat
    {
        [Key]
        public int IdIN_CauHoiKhaoSat { get; set; }

        [Required]
        public string? TieuDeCauHoi { get; set; }

        [Required]
        public string? CauHoi { get; set; }

        public int IdIN_NhomCauHoiKhaoSat { get; set; }

        [ForeignKey("IdIN_NhomCauHoiKhaoSat")]
        public IN_NhomCauHoiKhaoSat? NhomCauHoiKhaoSat { get; set; }
    }
}
