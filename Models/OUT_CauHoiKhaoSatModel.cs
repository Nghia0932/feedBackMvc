using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedBackMvc.Models
{
    public class OUT_CauHoiKhaoSat
    {
        [Key]
        public int IdOUT_CauHoiKhaoSat { get; set; }

        [Required]
        public string? TieuDeCauHoi { get; set; }

        [Required]
        public string? CauHoi { get; set; }

        public int IdOUT_NhomCauHoiKhaoSat { get; set; }

        [ForeignKey("IdOUT_NhomCauHoiKhaoSat")]
        public OUT_NhomCauHoiKhaoSat? NhomCauHoiKhaoSat { get; set; }
    }
}
